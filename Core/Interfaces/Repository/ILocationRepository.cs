using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        public Task<Location> GetLocationByName(string locationName);
    }
}