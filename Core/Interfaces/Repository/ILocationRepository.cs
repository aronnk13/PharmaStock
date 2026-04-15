using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface ILocationRepository
    {
        Task<Location?> GetLocationById(int id);
        Task<IEnumerable<Location>> GetLocations();
        Task<Location> CreateLocation(Location location);
        Task<bool> UpdateLocation(Location location);
        Task<bool> DeleteLocation(int id);
        Task<bool> IsLocationExists(string name, int locationTypeId, int? excludeId = null);
        Task<bool> HasChildLocations(int locationId);
        Task<string?> GetLocationTypeName(int locationTypeId);
        Task<IEnumerable<LocationType>> GetAllLocationTypesAsync();
        Task<LocationType?> GetLocationTypeByIdAsync(int id);
    }
}
