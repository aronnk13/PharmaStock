using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ReplenishmentRepository : GenericRepository<ReplenishmentRequest>, IReplenishmentRepository
    {
        public ReplenishmentRepository(PharmaStockContext context) : base(context) { }

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

        public async Task<IEnumerable<ReplenishmentRule>> GetAllRulesAsync()
        {
            return await _pharmaStockContext.ReplenishmentRules
                .Include(r => r.Location)
                .Include(r => r.Item).ThenInclude(i => i.Drug)
                .ToListAsync();
        }
    }
}
