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
                .Include(g => g.PurchaseOrder)
                    .ThenInclude(po => po.PurchaseOrderStatus)
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
                .Include(i => i.Item)
                    .ThenInclude(item => item.Drug)
                        .ThenInclude(d => d.ControlClassNavigation)
                .Include(i => i.Item)
                    .ThenInclude(item => item.Drug)
                        .ThenInclude(d => d.StorageClassNavigation)
                .Include(i => i.GoodsReceipt)
                    .ThenInclude(gr => gr.StatusNavigation)
                .Include(i => i.PurchaseOrderItem)
                .FirstOrDefaultAsync(i => i.GoodsReceiptItemId == goodsReceiptItemId);
        }

        public async Task<(List<GRNItemListDTO>, int)> GetFilteredItemsAsync(int goodsReceiptId, GRNItemFilterDTO filter)
        {
            var query = _context.GoodsReceiptItems
                .Include(i => i.Item)
                    .ThenInclude(item => item.Drug)
                        .ThenInclude(d => d.ControlClassNavigation)
                .Where(i => i.GoodsReceiptId == goodsReceiptId)
                .AsQueryable();

            if (filter.ItemId.HasValue)
                query = query.Where(i => i.ItemId == filter.ItemId.Value);

            if (filter.BatchNumber.HasValue)
                query = query.Where(i => i.BatchNumber == filter.BatchNumber.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(i => new GRNItemListDTO
                {
                    GoodsReceiptItemId = i.GoodsReceiptItemId,
                    ItemId = i.ItemId,
                    DrugName = i.Item.Drug.GenericName,
                    ControlClass = i.Item.Drug.ControlClassNavigation.Class,
                    BatchNumber = i.BatchNumber,
                    ExpiryDate = i.ExpiryDate,
                    ReceivedQty = i.ReceivedQty,
                    AcceptedQty = i.AcceptedQty,
                    RejectedQty = i.RejectedQty,
                    Reason = i.Reason
                })
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> IsDuplicateBatchAsync(int goodsReceiptId, int itemId, int batchNumber, int? excludeId = null)
        {
            return await _context.GoodsReceiptItems.AnyAsync(i =>
                i.GoodsReceiptId == goodsReceiptId &&
                i.ItemId == itemId &&
                i.BatchNumber == batchNumber &&
                (!excludeId.HasValue || i.GoodsReceiptItemId != excludeId.Value));
        }

        public async Task<int> CountActiveItemsInGrnAsync(int goodsReceiptId)
        {
            return await _context.GoodsReceiptItems
                .CountAsync(i => i.GoodsReceiptId == goodsReceiptId);
        }

        public async Task<bool> HasActiveTaskAsync(int goodsReceiptItemId)
        {
            return await _context.Tasks
                .AnyAsync(t => t.GoodsReceiptItemId == goodsReceiptItemId && t.Status == false);
        }

    }
}
