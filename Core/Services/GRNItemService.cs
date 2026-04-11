using PharmaStock.Core.DTO.GRNItem;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class GRNItemService : IGRNItemService
    {
        private readonly IGRNItemRepository _repository;

        public GRNItemService(IGRNItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<GRNItemResponseDTO> CreateAsync(int goodsReceiptId, CreateGRNItemDTO dto)
        {
            var grn = await _repository.GetGoodsReceiptWithDetailsAsync(goodsReceiptId)
                ?? throw new KeyNotFoundException("GRI_ERR_001");

            if (grn.Status != 1)
                throw new InvalidOperationException("GRI_ERR_002");

            if (grn.PurchaseOrder.PurchaseOrderStatusId != 2 && grn.PurchaseOrder.PurchaseOrderStatusId != 3)
                throw new InvalidOperationException("GRI_ERR_012");

            if (dto.BatchNumber <= 0)
                throw new ArgumentException("GRI_ERR_005");

            if (dto.ExpiryDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("GRI_ERR_006");

            if (dto.ReceivedQty <= 0)
                throw new ArgumentException("GRI_ERR_007");

            if (dto.AcceptedQty + dto.RejectedQty != dto.ReceivedQty)
                throw new InvalidOperationException("GRI_ERR_008");

            if (dto.RejectedQty > 0 && string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("GRI_ERR_009");

            var item = await _repository.GetItemByIdAsync(dto.ItemId)
                ?? throw new ArgumentException("GRI_ERR_003");

            if (!item.Status)
                throw new ArgumentException("GRI_ERR_003");

            var poItem = await _repository.GetPurchaseItemByIdAsync(dto.PurchaseOrderItemId)
                ?? throw new ArgumentException("GRI_ERR_004");

            if (poItem.PurchaseOrderId != grn.PurchaseOrderId)
                throw new ArgumentException("GRI_ERR_004");

            if (await _repository.IsDuplicateBatchAsync(goodsReceiptId, dto.ItemId, dto.BatchNumber))
                throw new InvalidOperationException("GRI_ERR_011");

            bool overShipmentFlag = dto.ReceivedQty > poItem.OrderedQty;

            var entity = new GoodsReceiptItem
            {
                GoodsReceiptId = goodsReceiptId,
                PurchaseOrderItemId = dto.PurchaseOrderItemId,
                ItemId = dto.ItemId,
                BatchNumber = dto.BatchNumber,
                ExpiryDate = dto.ExpiryDate,
                ReceivedQty = dto.ReceivedQty,
                AcceptedQty = dto.AcceptedQty,
                RejectedQty = dto.RejectedQty,
                Reason = dto.Reason
            };

            await _repository.AddAsync(entity);

            return new GRNItemResponseDTO
            {
                GoodsReceiptItemId = entity.GoodsReceiptItemId,
                GoodsReceiptId = entity.GoodsReceiptId,
                PurchaseOrderItemId = entity.PurchaseOrderItemId,
                ItemId = entity.ItemId,
                DrugName = item.Drug.GenericName,
                BrandName = item.Drug.BrandName,
                ControlClass = item.Drug.ControlClassNavigation.Class,
                StorageClass = item.Drug.StorageClassNavigation.Class,
                BatchNumber = entity.BatchNumber,
                ExpiryDate = entity.ExpiryDate,
                ReceivedQty = entity.ReceivedQty,
                AcceptedQty = entity.AcceptedQty,
                RejectedQty = entity.RejectedQty,
                Reason = entity.Reason,
                OverShipmentFlag = overShipmentFlag
            };
        }

        public async Task<GRNItemsPagedResponseDTO> GetAllAsync(int goodsReceiptId, GRNItemFilterDTO filter)
        {
            var grn = await _repository.GetGoodsReceiptWithDetailsAsync(goodsReceiptId)
                ?? throw new KeyNotFoundException("GRI_ERR_001");

            var (items, totalCount) = await _repository.GetFilteredItemsAsync(goodsReceiptId, filter);

            return new GRNItemsPagedResponseDTO
            {
                GoodsReceiptId = goodsReceiptId,
                GrnStatus = grn.StatusNavigation.Status,
                TotalItems = totalCount,
                Items = items,
                Page = filter.Page,
                Size = filter.PageSize
            };
        }

        public async Task<GRNItemResponseDTO> GetByIdAsync(int goodsReceiptId, int goodsReceiptItemId)
        {
            var entity = await _repository.GetItemWithDetailsAsync(goodsReceiptItemId)
                ?? throw new KeyNotFoundException("GRI_ERR_022");

            if (entity.GoodsReceiptId != goodsReceiptId)
                throw new KeyNotFoundException("GRI_ERR_022");

            return new GRNItemResponseDTO
            {
                GoodsReceiptItemId = entity.GoodsReceiptItemId,
                GoodsReceiptId = entity.GoodsReceiptId,
                PurchaseOrderItemId = entity.PurchaseOrderItemId,
                ItemId = entity.ItemId,
                DrugName = entity.Item.Drug.GenericName,
                BrandName = entity.Item.Drug.BrandName,
                ControlClass = entity.Item.Drug.ControlClassNavigation.Class,
                StorageClass = entity.Item.Drug.StorageClassNavigation.Class,
                BatchNumber = entity.BatchNumber,
                ExpiryDate = entity.ExpiryDate,
                ReceivedQty = entity.ReceivedQty,
                AcceptedQty = entity.AcceptedQty,
                RejectedQty = entity.RejectedQty,
                Reason = entity.Reason
            };
        }

        public async Task<GRNItemResponseDTO> UpdateAsync(int goodsReceiptId, int goodsReceiptItemId, UpdateGRNItemDTO dto)
        {
            var entity = await _repository.GetItemWithDetailsAsync(goodsReceiptItemId)
                ?? throw new KeyNotFoundException("GRI_ERR_022");

            if (entity.GoodsReceiptId != goodsReceiptId)
                throw new KeyNotFoundException("GRI_ERR_022");

            if (entity.GoodsReceipt.Status != 1)
                throw new InvalidOperationException("GRI_ERR_030");

            if (dto.ExpiryDate != entity.ExpiryDate && string.IsNullOrWhiteSpace(dto.ChangeReason))
                throw new ArgumentException("GRI_ERR_032");

            if (dto.ReceivedQty != entity.ReceivedQty && (entity.AcceptedQty > 0 || entity.RejectedQty > 0))
                throw new ArgumentException("GRI_ERR_035");

            if (dto.AcceptedQty + dto.RejectedQty != dto.ReceivedQty)
                throw new InvalidOperationException("GRI_ERR_031");

            if (dto.RejectedQty < entity.RejectedQty && string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("GRI_ERR_034");

            entity.BatchNumber = dto.BatchNumber;
            entity.ExpiryDate = dto.ExpiryDate;
            entity.ReceivedQty = dto.ReceivedQty;
            entity.AcceptedQty = dto.AcceptedQty;
            entity.RejectedQty = dto.RejectedQty;
            entity.Reason = dto.Reason;

            await _repository.UpdateAsync(entity);

            return new GRNItemResponseDTO
            {
                GoodsReceiptItemId = entity.GoodsReceiptItemId,
                GoodsReceiptId = entity.GoodsReceiptId,
                PurchaseOrderItemId = entity.PurchaseOrderItemId,
                ItemId = entity.ItemId,
                DrugName = entity.Item.Drug.GenericName,
                BrandName = entity.Item.Drug.BrandName,
                ControlClass = entity.Item.Drug.ControlClassNavigation.Class,
                StorageClass = entity.Item.Drug.StorageClassNavigation.Class,
                BatchNumber = entity.BatchNumber,
                ExpiryDate = entity.ExpiryDate,
                ReceivedQty = entity.ReceivedQty,
                AcceptedQty = entity.AcceptedQty,
                RejectedQty = entity.RejectedQty,
                Reason = entity.Reason
            };
        }

        public async Task<GRNItemDeleteResponseDTO> DeleteAsync(int goodsReceiptId, int goodsReceiptItemId, DeleteGRNItemDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("GRI_ERR_041");

            var entity = await _repository.GetItemWithDetailsAsync(goodsReceiptItemId)
                ?? throw new KeyNotFoundException("GRI_ERR_045");

            if (entity.GoodsReceiptId != goodsReceiptId)
                throw new KeyNotFoundException("GRI_ERR_045");

            if (entity.GoodsReceipt.Status != 1)
                throw new InvalidOperationException("GRI_ERR_040");

            var activeCount = await _repository.CountActiveItemsInGrnAsync(goodsReceiptId);
            if (activeCount <= 1)
                throw new InvalidOperationException("GRI_ERR_043");

            if (await _repository.HasActiveTaskAsync(goodsReceiptItemId))
                throw new InvalidOperationException("GRI_ERR_044");

            await _repository.DeleteAsync(goodsReceiptItemId);

            return new GRNItemDeleteResponseDTO
            {
                GoodsReceiptItemId = goodsReceiptItemId,
                Status = "Inactive",
                Reason = dto.Reason,
                Message = "GoodsReceiptItem deleted successfully."
            };
        }
    }
}
