using PharmaStock.Core.DTO.QCO;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IRecallNoticeService
    {
        Task<IEnumerable<RecallNoticeDTO>> GetAllAsync();
        Task<RecallNoticeDTO?> GetByIdAsync(int id);
        Task<RecallNoticeDTO> CreateAsync(CreateRecallNoticeDTO dto);
        Task<bool> ResolveAsync(int id);   // mark recall as resolved (Status = false)
    }
}
