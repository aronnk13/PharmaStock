using PharmaStock.Core.DTO.QCO;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IRecallNoticeService
    {
        Task<IEnumerable<RecallNoticeDTO>> GetAllAsync();
        Task<RecallNoticeDTO?> GetByIdAsync(int id);
    }
}
