using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IReplenishmentRuleRepository : IGenericRepository<ReplenishmentRule>
    {
        public Task<bool> IsRuleExist(int locationId, int itemId);
    }
}