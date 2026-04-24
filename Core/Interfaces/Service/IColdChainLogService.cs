using PharmaStock.Core.DTO.QCO;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IColdChainLogService
    {
        Task<IEnumerable<ColdChainLogDTO>> GetAllAsync();
        Task<ColdChainLogDTO> CreateAsync(CreateColdChainLogDTO dto);
    }
}
