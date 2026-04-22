using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IExpiryWatchRepository : IGenericRepository<ExpiryWatch>
    {
        Task<IEnumerable<ExpiryWatch>> GetActiveWatchesAsync();
        Task<IEnumerable<ExpiryWatch>> GetNearExpiryAsync(int daysThreshold);
    }
}
