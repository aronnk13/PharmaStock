using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class StockAdjustmentRepository : GenericRepository<StockAdjustment>, IStockAdjustmentRepository
    {
        public StockAdjustmentRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<StockAdjustment>> GetAllWithDetailsAsync()
        {
            return await _pharmaStockContext.StockAdjustments
                .Include(s => s.Location)
                .Include(s => s.Item).ThenInclude(i => i.Drug)
                .Include(s => s.InventoryLot)
                .Include(s => s.ReasonCodeNavigation)
                .Include(s => s.ApprovedByNavigation)
                .OrderByDescending(s => s.PostedDate)
                .ToListAsync();
        }

        public async Task<int> CountRecentAsync(int days)
        {
            var since = DateTime.UtcNow.AddDays(-days);
            return await _pharmaStockContext.StockAdjustments
                .Where(s => s.PostedDate >= since)
                .CountAsync();
        }
    }
}
