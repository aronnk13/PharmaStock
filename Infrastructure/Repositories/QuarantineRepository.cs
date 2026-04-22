using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class QuarantineRepository : GenericRepository<QuarantaineAction>, IQuarantineRepository
    {
        public QuarantineRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<QuarantaineAction>> GetAllWithDetailsAsync()
        {
            return await _pharmaStockContext.QuarantaineActions
                .Include(q => q.InventoryLot).ThenInclude(l => l.Item).ThenInclude(i => i.Drug)
                .Include(q => q.StatusNavigation)
                .OrderByDescending(q => q.QuarantineDate)
                .ToListAsync();
        }

        public async Task<QuarantaineAction?> GetByIdWithDetailsAsync(int id)
        {
            return await _pharmaStockContext.QuarantaineActions
                .Include(q => q.InventoryLot).ThenInclude(l => l.Item).ThenInclude(i => i.Drug)
                .Include(q => q.StatusNavigation)
                .FirstOrDefaultAsync(q => q.QuarantaineActionId == id);
        }

        public async Task<int> CountActiveAsync()
        {
            return await _pharmaStockContext.QuarantaineActions
                .Where(q => q.Status == 1)
                .CountAsync();
        }

        public async Task<IEnumerable<QuarantaineAction>> GetRecentAsync(int count)
        {
            return await _pharmaStockContext.QuarantaineActions
                .Include(q => q.InventoryLot).ThenInclude(l => l.Item).ThenInclude(i => i.Drug)
                .Include(q => q.StatusNavigation)
                .OrderByDescending(q => q.QuarantineDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
