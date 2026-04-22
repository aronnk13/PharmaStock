using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class DispenseRepository : GenericRepository<DispenseRef>, IDispenseRepository
    {
        public DispenseRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<DispenseRef>> GetAllWithDetailsAsync()
        {
            return await _pharmaStockContext.DispenseRefs
                .Include(d => d.Location)
                .Include(d => d.Item).ThenInclude(i => i.Drug)
                .Include(d => d.InventoryLot)
                .Include(d => d.DestinationNavigation)
                .OrderByDescending(d => d.DispenseDate)
                .ToListAsync();
        }

        public async Task<DispenseRef?> GetByIdWithDetailsAsync(int id)
        {
            return await _pharmaStockContext.DispenseRefs
                .Include(d => d.Location)
                .Include(d => d.Item).ThenInclude(i => i.Drug)
                .Include(d => d.InventoryLot)
                .Include(d => d.DestinationNavigation)
                .FirstOrDefaultAsync(d => d.DispenseRefId == id);
        }

        public async Task<IEnumerable<DispenseRef>> GetByLocationAsync(int locationId)
        {
            return await _pharmaStockContext.DispenseRefs
                .Include(d => d.Location)
                .Include(d => d.Item).ThenInclude(i => i.Drug)
                .Include(d => d.InventoryLot)
                .Include(d => d.DestinationNavigation)
                .Where(d => d.LocationId == locationId)
                .OrderByDescending(d => d.DispenseDate)
                .ToListAsync();
        }

        public async Task<int> CountTodayByLocationAsync(int locationId)
        {
            var today = DateTime.UtcNow.Date;
            return await _pharmaStockContext.DispenseRefs
                .Where(d => d.LocationId == locationId && d.DispenseDate.Date >= today)
                .CountAsync();
        }

        public async Task<IEnumerable<DispenseRef>> GetRecentByLocationAsync(int locationId, int count)
        {
            return await _pharmaStockContext.DispenseRefs
                .Include(d => d.Item).ThenInclude(i => i.Drug)
                .Include(d => d.DestinationNavigation)
                .Where(d => d.LocationId == locationId)
                .OrderByDescending(d => d.DispenseDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
