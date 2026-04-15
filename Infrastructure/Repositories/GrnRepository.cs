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
                    ReceivedBy = g.ReceivedBy
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

        public async Task<string?> GetUsernameByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Username)
                .FirstOrDefaultAsync();
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

            if (!string.IsNullOrEmpty(filter.ReceivedBy))
                query = query.Where(g => g.ReceivedBy == filter.ReceivedBy);

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
                    ReceivedBy = g.ReceivedBy
                })
                .ToListAsync();

            return (grns, totalCount);
        }
    }
}
