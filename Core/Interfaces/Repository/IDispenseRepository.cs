using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IDispenseRepository : IGenericRepository<DispenseRef>
    {
        Task<IEnumerable<DispenseRef>> GetAllWithDetailsAsync();
        Task<DispenseRef?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<DispenseRef>> GetByLocationAsync(int locationId);
        Task<int> CountTodayByLocationAsync(int locationId);
        Task<IEnumerable<DispenseRef>> GetRecentByLocationAsync(int locationId, int count);
        Task<int> CountTodayAsync();
        Task<IEnumerable<DispenseRef>> GetRecentAsync(int count);
    }
}
