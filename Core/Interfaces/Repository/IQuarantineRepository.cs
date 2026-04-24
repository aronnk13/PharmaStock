using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IQuarantineRepository : IGenericRepository<QuarantaineAction>
    {
        Task<IEnumerable<QuarantaineAction>> GetAllWithDetailsAsync();
        Task<QuarantaineAction?> GetByIdWithDetailsAsync(int id);
        Task<int> CountActiveAsync();
        Task<IEnumerable<QuarantaineAction>> GetRecentAsync(int count);
    }
}
