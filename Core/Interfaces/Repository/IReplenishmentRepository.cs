using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IReplenishmentRepository : IGenericRepository<ReplenishmentRequest>
    {
        Task<IEnumerable<ReplenishmentRequest>> GetByStatusAsync(int status);
        Task<IEnumerable<ReplenishmentRequest>> GetByLocationAsync(int locationId);
        Task<IEnumerable<ReplenishmentRule>> GetAllRulesAsync();
    }
}
