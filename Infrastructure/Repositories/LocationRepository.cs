using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private readonly PharmaStockContext _context;

        public LocationRepository(PharmaStockContext pharmaStockContext)
            : base(pharmaStockContext)
        {
            _context = pharmaStockContext;
        }

        public async Task<Location?> GetLocationById(int id)
        {
            return await _context.Locations.FindAsync(id);
        }

        public async Task<IEnumerable<Location>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<Location> CreateLocation(Location location)
        {
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<bool> UpdateLocation(Location location)
        {
            _context.Locations.Update(location);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return false;
            _context.Locations.Remove(location);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsLocationExists(string name, int locationTypeId, int? excludeId = null)
        {
            return await _context.Locations.AnyAsync(l =>
                l.Name == name &&
                l.LocationTypeId == locationTypeId &&
                (!excludeId.HasValue || l.LocationId != excludeId.Value));
        }

        public async Task<bool> HasChildLocations(int locationId)
        {
            return await _context.Locations.AnyAsync(l => l.ParentLocationId == locationId);
        }

        public async Task<string?> GetLocationTypeName(int locationTypeId)
        {
            var locationType = await _context.LocationTypes.FindAsync(locationTypeId);
            return locationType?.Type;
        }
    }
}
