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

        public async Task<GRNItemResponseDTO> CreateAsync(CreateGRNItemDTO dto)
        {
            var grn = await _repository.GetGoodsReceiptWithDetailsAsync(dto.GoodsReceiptId)
                ?? throw new KeyNotFoundException("GRN not found");

            if (grn.Status != 1)
                throw new InvalidOperationException("GRN not Open");

            var item = await _repository.GetItemByIdAsync(dto.ItemId)
                ?? throw new KeyNotFoundException("Item not found");

            var poItem = await _repository.GetPurchaseItemByIdAsync(dto.PurchaseOrderItemId)
                ?? throw new KeyNotFoundException("PurchaseItem not found");

            bool overShipmentFlag = dto.ReceivedQty > poItem.OrderedQty;

            var entity = new GoodsReceiptItem
            {
                GoodsReceiptId = dto.GoodsReceiptId,
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

        public async Task<object> GetAsync(GRNItemFilterDTO filter)
        {
            if (filter.GoodsReceiptItemId.HasValue)
            {
                var entity = await _repository.GetItemWithDetailsAsync(filter.GoodsReceiptItemId.Value)
                    ?? throw new KeyNotFoundException("GRNItem not found");

                if (entity.GoodsReceiptId != filter.GoodsReceiptId)
                    throw new KeyNotFoundException("GRNItem not found");

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

            var grn = await _repository.GetGoodsReceiptWithDetailsAsync(filter.GoodsReceiptId)
                ?? throw new KeyNotFoundException("GRN not found");

            var (items, totalCount) = await _repository.GetFilteredItemsAsync(filter.GoodsReceiptId, filter);

            return new GRNItemsPagedResponseDTO
            {
                GoodsReceiptId = filter.GoodsReceiptId,
                GrnStatus = grn.StatusNavigation.Status,
                TotalItems = totalCount,
                Items = items,
                Page = filter.Page,
                Size = filter.PageSize
            };
        }

        public async Task<GRNItemResponseDTO> UpdateAsync(UpdateGRNItemDTO dto)
        {
            var entity = await _repository.GetItemWithDetailsAsync(dto.GoodsReceiptItemId)
                ?? throw new KeyNotFoundException("GRNItem not found");

            if (entity.GoodsReceiptId != dto.GoodsReceiptId)
                throw new KeyNotFoundException("GRNItem not found");

            if (entity.GoodsReceipt.Status != 1)
                throw new InvalidOperationException("GRN not Open");

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
    }
}
