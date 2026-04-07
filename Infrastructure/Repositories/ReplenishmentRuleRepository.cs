using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ReplenishmentRuleRepository : GenericRepository<ReplenishmentRule>, IReplenishmentRuleRepository
    {
        private readonly PharmaStockContext _pharmaStockContext;
        public ReplenishmentRuleRepository(PharmaStockContext pharmaStockContext)
               : base(pharmaStockContext)
        {
            _pharmaStockContext = pharmaStockContext;
        }

        public async Task<bool> IsRuleExistAsync(int locationId, int itemId)
        {
            return await _pharmaStockContext.ReplenishmentRules.AnyAsync(r => r.LocationId == locationId && r.ItemId == itemId);
        }
    }
}