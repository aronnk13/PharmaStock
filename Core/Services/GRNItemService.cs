using PharmaStock.Core.DTO.GRNItem;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;
using SystemTask = System.Threading.Tasks.Task;

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
            // BatchNumber is stored as INT in the DB — validate it is numeric
            if (!int.TryParse(dto.BatchNumber?.Trim(), out var batchNumberInt))
                throw new ArgumentException("INVALID_BATCH_NUMBER: Batch number must be numeric (e.g. 2026, 20260424).");

            var grn = await _repository.GetGoodsReceiptWithDetailsAsync(dto.GoodsReceiptId)
                ?? throw new KeyNotFoundException("GRN not found");

            if (grn.Status != 1)
                throw new InvalidOperationException("GRN not Open");

            var poItem = await _repository.GetPurchaseItemByIdAsync(dto.PurchaseOrderItemId)
                ?? throw new KeyNotFoundException("PurchaseItem not found");

            var item = await _repository.GetItemByIdAsync(poItem.ItemId)
                ?? throw new KeyNotFoundException("Item not found");

            if (await _repository.IsDuplicateBatchAsync(dto.GoodsReceiptId, poItem.ItemId, batchNumberInt))
                throw new InvalidOperationException("Duplicate batch");

            bool overShipmentFlag = dto.ReceivedQty > poItem.OrderedQty;

            var entity = new GoodsReceiptItem
            {
                GoodsReceiptId      = dto.GoodsReceiptId,
                PurchaseOrderItemId = dto.PurchaseOrderItemId,
                ItemId              = poItem.ItemId,
                BatchNumber         = batchNumberInt,           // int
                ExpiryDate          = dto.ExpiryDate,
                ReceivedQty         = dto.ReceivedQty,
                AcceptedQty         = dto.AcceptedQty,
                RejectedQty         = dto.RejectedQty,
                Reason              = dto.Reason
            };

            await _repository.AddAsync(entity);

            return new GRNItemResponseDTO
            {
                BatchNumber      = entity.BatchNumber.ToString(), // int → string for response
                ExpiryDate       = entity.ExpiryDate,
                ReceivedQty      = entity.ReceivedQty,
                AcceptedQty      = entity.AcceptedQty,
                RejectedQty      = entity.RejectedQty,
                Reason           = entity.Reason,
                OverShipmentFlag = overShipmentFlag
            };
        }

        public async Task<GRNItemsPagedResponseDTO> GetAsync(GRNItemFilterDTO filter)
        {
            var (items, totalCount) = await _repository.GetFilteredItemsAsync(filter);

            return new GRNItemsPagedResponseDTO
            {
                TotalItems = totalCount,
                Items = items,
                Page = filter.Page,
                Size = filter.PageSize
            };
        }

        public async SystemTask UpdateAsync(UpdateGRNItemDTO dto)
        {
            if (!int.TryParse(dto.BatchNumber?.Trim(), out var batchNumberInt))
                throw new ArgumentException("INVALID_BATCH_NUMBER: Batch number must be numeric.");

            var entity = await _repository.GetItemWithDetailsAsync(dto.GoodsReceiptItemId)
                ?? throw new KeyNotFoundException("GRNItem not found");

            if (entity.GoodsReceiptId != dto.GoodsReceiptId)
                throw new KeyNotFoundException("GRNItem not found");

            if (entity.GoodsReceipt.Status != 1)
                throw new InvalidOperationException("GRN not Open");

            entity.BatchNumber = batchNumberInt;   // int
            entity.ExpiryDate  = dto.ExpiryDate;
            entity.ReceivedQty = dto.ReceivedQty;
            entity.AcceptedQty = dto.AcceptedQty;
            entity.RejectedQty = dto.RejectedQty;
            entity.Reason      = dto.Reason;

            await _repository.UpdateAsync(entity);
        }
    }
}
