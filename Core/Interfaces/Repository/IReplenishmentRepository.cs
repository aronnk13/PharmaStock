using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IReplenishmentRepository : IGenericRepository<ReplenishmentRequest>
    {
        Task<IEnumerable<ReplenishmentRequest>> GetAllWithDetailsAsync();
        Task<ReplenishmentRequest?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<ReplenishmentRequest>> GetByStatusAsync(int status);
        Task<IEnumerable<ReplenishmentRequest>> GetByLocationAsync(int locationId);
        Task<bool> HasOpenRequestAsync(int locationId, int itemId);

        Task<IEnumerable<ReplenishmentRule>> GetAllRulesAsync();
        Task<ReplenishmentRule?> GetRuleByIdAsync(int id);
        System.Threading.Tasks.Task AddRuleAsync(ReplenishmentRule rule);
        Task<bool> UpdateRuleAsync(ReplenishmentRule rule);
        Task<bool> DeleteRuleAsync(int id);
    }
}
