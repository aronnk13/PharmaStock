using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.GoodsReceipt;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class GrnRepository : GenericRepository<GoodsReciept>, IGrnRepository
    {
        private readonly PharmaStockContext _context;

        public GrnRepository(PharmaStockContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            return await _context.PurchaseOrders
                .Include(po => po.PurchaseOrderStatus)
                .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId);
        }

        public async Task<bool> HasGrnItemsAsync(int grnId)
        {
            return await _context.GoodsReceiptItems
                .AnyAsync(i => i.GoodsReceiptId == grnId);
        }

        public async Task<GetGrnDTO?> GetGrnDtoByIdAsync(int grnId)
        {
            return await _context.GoodsReciepts
                .Where(g => g.GoodsRecieptId == grnId)
                .Select(g => new GetGrnDTO
                {
                    GoodsReceiptId = g.GoodsRecieptId,
                    PurchaseOrderId = g.PurchaseOrderId,
                    ReceivedDate = g.ReceivedDate,
                    StatusId = g.Status,
                    Status = g.StatusNavigation.Status,
                    ReceivedByUserId = g.ReceivedBy,
                    ReceivedByUserName = g.ReceivedByNavigation.Username
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetReceivablePoStatusIdsAsync()
        {
            return await _context.PurchaseOrderStatuses
                .Where(s => s.Status == "Approved" || s.Status == "PartiallyReceived")
                .Select(s => s.PurchaseOrderStatusId)
                .ToListAsync();
        }

        public async Task<GoodsReceiptStatus?> GetGrnStatusByCodeAsync(string statusCode)
        {
            return await _context.GoodsReceiptStatuses
                .FirstOrDefaultAsync(s => s.Status == statusCode);
        }

        public async Task<(List<GetGrnDTO>, int)> GetGrnsByFilterAsync(GrnFilterDTO filter)
        {
            var query = _context.GoodsReciepts.AsQueryable();

            if (filter.StatusId.HasValue)
                query = query.Where(g => g.Status == filter.StatusId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(g => g.ReceivedDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(g => g.ReceivedDate <= filter.ToDate.Value);

            if (filter.ReceivedBy.HasValue)
                query = query.Where(g => g.ReceivedBy == filter.ReceivedBy.Value);

            var totalCount = await query.CountAsync();

            var pageSize = filter.PageSize > 100 ? 100 : filter.PageSize;

            var grns = await query
                .Skip((filter.Page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GetGrnDTO
                {
                    GoodsReceiptId = g.GoodsRecieptId,
                    PurchaseOrderId = g.PurchaseOrderId,
                    ReceivedDate = g.ReceivedDate,
                    StatusId = g.Status,
                    Status = g.StatusNavigation.Status,
                    ReceivedByUserId = g.ReceivedBy,
                    ReceivedByUserName = g.ReceivedByNavigation.Username
                })
                .ToListAsync();

            return (grns, totalCount);
        }
    }
}
