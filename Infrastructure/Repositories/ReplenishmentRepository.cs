using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ReplenishmentRepository : GenericRepository<ReplenishmentRequest>, IReplenishmentRepository
    {
        public ReplenishmentRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<ReplenishmentRequest>> GetAllWithDetailsAsync()
        {
            return await _pharmaStockContext.ReplenishmentRequests
                .Include(r => r.Location)
                .Include(r => r.Item).ThenInclude(i => i.Drug)
                .Include(r => r.StatusNavigation)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        public async Task<ReplenishmentRequest?> GetByIdWithDetailsAsync(int id)
        {
            return await _pharmaStockContext.ReplenishmentRequests
                .Include(r => r.Location)
                .Include(r => r.Item).ThenInclude(i => i.Drug)
                .Include(r => r.StatusNavigation)
                .FirstOrDefaultAsync(r => r.ReplenishmentRequestId == id);
        }

        public async Task<IEnumerable<ReplenishmentRequest>> GetByStatusAsync(int status)
        {
            return await _pharmaStockContext.ReplenishmentRequests
                .Include(r => r.Location)
                .Include(r => r.Item).ThenInclude(i => i.Drug)
                .Include(r => r.StatusNavigation)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReplenishmentRequest>> GetByLocationAsync(int locationId)
        {
            return await _pharmaStockContext.ReplenishmentRequests
                .Include(r => r.Location)
                .Include(r => r.Item).ThenInclude(i => i.Drug)
                .Include(r => r.StatusNavigation)
                .Where(r => r.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<bool> HasOpenRequestAsync(int locationId, int itemId)
        {
            return await _pharmaStockContext.ReplenishmentRequests
                .AnyAsync(r => r.LocationId == locationId && r.ItemId == itemId && r.Status == 1);
        }

        // ── Rules ──────────────────────────────────────────────────────────────

        public async Task<IEnumerable<ReplenishmentRule>> GetAllRulesAsync()
        {
            return await _pharmaStockContext.ReplenishmentRules
                .Include(r => r.Location)
                .Include(r => r.Item).ThenInclude(i => i.Drug)
                .ToListAsync();
        }

        public async Task<ReplenishmentRule?> GetRuleByIdAsync(int id)
        {
            return await _pharmaStockContext.ReplenishmentRules
                .Include(r => r.Location)
                .Include(r => r.Item).ThenInclude(i => i.Drug)
                .FirstOrDefaultAsync(r => r.ReplenishmentRuleId == id);
        }

        public async System.Threading.Tasks.Task AddRuleAsync(ReplenishmentRule rule)
        {
            _pharmaStockContext.ReplenishmentRules.Add(rule);
            await _pharmaStockContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateRuleAsync(ReplenishmentRule rule)
        {
            _pharmaStockContext.ReplenishmentRules.Update(rule);
            return await _pharmaStockContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRuleAsync(int id)
        {
            var rule = await _pharmaStockContext.ReplenishmentRules.FindAsync(id);
            if (rule == null) return false;
            _pharmaStockContext.ReplenishmentRules.Remove(rule);
            return await _pharmaStockContext.SaveChangesAsync() > 0;
        }
    }
}
