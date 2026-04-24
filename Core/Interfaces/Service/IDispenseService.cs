using PharmaStock.Core.DTO.Pharmacist;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IDispenseService
    {
        Task<IEnumerable<DispenseRefDTO>> GetAllAsync();
        Task<IEnumerable<DispenseRefDTO>> GetByLocationAsync(int locationId);
        Task<DispenseRefDTO?> GetByIdAsync(int id);
        Task<DispenseRefDTO> CreateAsync(CreateDispenseRefDTO dto);
    }
}
