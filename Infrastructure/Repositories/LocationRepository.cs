using Microsoft.EntityFrameworkCore;

using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private readonly PharmaStockContext _pharmaStockContext;
        public LocationRepository(PharmaStockContext pharmaStockContext)
             : base(pharmaStockContext)
        {
            _pharmaStockContext = pharmaStockContext;
        }
        public async Task<Location> GetLocationByName(string locationName)
        {
            return await _pharmaStockContext.Locations.FirstOrDefaultAsync(l => l.Name == locationName);
        }
    }
}