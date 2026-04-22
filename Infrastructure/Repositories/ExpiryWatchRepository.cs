using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ExpiryWatchRepository : GenericRepository<ExpiryWatch>, IExpiryWatchRepository
    {
        public ExpiryWatchRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<ExpiryWatch>> GetActiveWatchesAsync()
        {
            return await _pharmaStockContext.ExpiryWatches
                .Include(e => e.InventoryLot)
                    .ThenInclude(il => il.Item)
                        .ThenInclude(i => i.Drug)
                .Where(e => e.Status == true)
                .OrderBy(e => e.InventoryLot.ExpiryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpiryWatch>> GetNearExpiryAsync(int daysThreshold)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var cutoff = today.AddDays(daysThreshold);

            return await _pharmaStockContext.ExpiryWatches
                .Include(e => e.InventoryLot)
                    .ThenInclude(il => il.Item)
                        .ThenInclude(i => i.Drug)
                .Where(e => e.Status == true && e.InventoryLot.ExpiryDate <= cutoff)
                .OrderBy(e => e.InventoryLot.ExpiryDate)
                .ToListAsync();
        }
    }
}
