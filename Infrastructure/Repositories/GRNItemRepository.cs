using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.GRNItem;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class GRNItemRepository : GenericRepository<GoodsReceiptItem>, IGRNItemRepository
    {
        private readonly PharmaStockContext _context;

        public GRNItemRepository(PharmaStockContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GoodsReciept?> GetGoodsReceiptWithDetailsAsync(int goodsReceiptId)
        {
            return await _context.GoodsReciepts
                .Include(g => g.StatusNavigation)
                .FirstOrDefaultAsync(g => g.GoodsRecieptId == goodsReceiptId);
        }

        public async Task<Item?> GetItemByIdAsync(int itemId)
        {
            return await _context.Items
                .Include(i => i.Drug)
                    .ThenInclude(d => d.ControlClassNavigation)
                .Include(i => i.Drug)
                    .ThenInclude(d => d.StorageClassNavigation)
                .FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        public async Task<PurchaseItem?> GetPurchaseItemByIdAsync(int purchaseItemId)
        {
            return await _context.PurchaseItems
                .FirstOrDefaultAsync(p => p.PurchaseItemId == purchaseItemId);
        }

        public async Task<GoodsReceiptItem?> GetItemWithDetailsAsync(int goodsReceiptItemId)
        {
            return await _context.GoodsReceiptItems
                .Include(i => i.GoodsReceipt)
                .FirstOrDefaultAsync(i => i.GoodsReceiptItemId == goodsReceiptItemId);
        }

        public async Task<GRNItemResponseDTO?> GetByIdAsync(int goodsReceiptItemId)
        {
            return await _context.GoodsReceiptItems
                .Include(i => i.PurchaseOrderItem)
                .Where(i => i.GoodsReceiptItemId == goodsReceiptItemId)
                .Select(i => new GRNItemResponseDTO
                {
                    GoodsReceiptItemId = i.GoodsReceiptItemId,
                    GoodsReceiptId = i.GoodsReceiptId,
                    PurchaseOrderItemId = i.PurchaseOrderItemId,
                    BatchNumber = i.BatchNumber,
                    ExpiryDate = i.ExpiryDate,
                    ReceivedQty = i.ReceivedQty,
                    AcceptedQty = i.AcceptedQty,
                    RejectedQty = i.RejectedQty,
                    Reason = i.Reason,
                    OverShipmentFlag = i.ReceivedQty > i.PurchaseOrderItem.OrderedQty
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(List<GRNItemResponseDTO>, int)> GetFilteredItemsAsync(GRNItemFilterDTO filter)
        {
            var query = _context.GoodsReceiptItems
                .Include(i => i.PurchaseOrderItem)
                .AsQueryable();

            if (filter.BatchNumber.HasValue)
                query = query.Where(i => i.BatchNumber == filter.BatchNumber.Value);

            if (filter.ExpiryDate.HasValue)
                query = query.Where(i => i.ExpiryDate == filter.ExpiryDate.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(i => new GRNItemResponseDTO
                {
                    GoodsReceiptItemId = i.GoodsReceiptItemId,
                    GoodsReceiptId = i.GoodsReceiptId,
                    PurchaseOrderItemId = i.PurchaseOrderItemId,
                    BatchNumber = i.BatchNumber,
                    ExpiryDate = i.ExpiryDate,
                    ReceivedQty = i.ReceivedQty,
                    AcceptedQty = i.AcceptedQty,
                    RejectedQty = i.RejectedQty,
                    Reason = i.Reason,
                    OverShipmentFlag = i.ReceivedQty > i.PurchaseOrderItem.OrderedQty
                })
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> IsDuplicateBatchAsync(int goodsReceiptId, int itemId, int batchNumber)
        {
            return await _context.GoodsReceiptItems.AnyAsync(i =>
                i.GoodsReceiptId == goodsReceiptId &&
                i.ItemId == itemId &&
                i.BatchNumber == batchNumber);
        }
    }
}
