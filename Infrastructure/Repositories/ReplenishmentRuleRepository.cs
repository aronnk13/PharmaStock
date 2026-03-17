using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ReplenishmentRuleRepository : GenericRepository<ReplenishmentRule>
    {
        public ReplenishmentRuleRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}