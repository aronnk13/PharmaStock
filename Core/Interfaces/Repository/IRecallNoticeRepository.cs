using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IRecallNoticeRepository : IGenericRepository<RecallNotice>
    {
        Task<IEnumerable<RecallNotice>> GetAllWithDetailsAsync();
        Task<int> CountOpenAsync();
        Task<IEnumerable<RecallNotice>> GetRecentAsync(int count);
    }
}
