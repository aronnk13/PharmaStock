using PharmaStock.Core.DTO.QCO;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IQuarantineService
    {
        Task<IEnumerable<QuarantineActionDTO>> GetAllAsync();
        Task<QuarantineActionDTO?> GetByIdAsync(int id);
        Task<QuarantineActionDTO> CreateAsync(CreateQuarantineActionDTO dto);
        Task<bool> ReleaseAsync(int id);
        Task<bool> DisposeAsync(int id);
    }
}
