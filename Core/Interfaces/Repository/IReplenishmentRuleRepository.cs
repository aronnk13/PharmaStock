using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IReplenishmentRuleRepository : IGenericRepository<ReplenishmentRule>
    {
        public Task<bool> IsRuleExistAsync(int locationId, int itemId);
    }
}