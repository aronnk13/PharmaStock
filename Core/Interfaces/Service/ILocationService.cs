using PharmaStock.Core.DTO.Location;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface ILocationService
    {
        Task<GetLocationDTO?> GetLocationbyId(int id);
        Task<IEnumerable<GetLocationDTO>> GetLocations();
        Task<GetLocationDTO> CreateLocation(CreateLocationDTO dto);
        Task<bool> UpdateLocation(UpdateLocationDTO dto);
        Task<bool> DeleteLocation(int id);
    }
}
