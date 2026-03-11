using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class InventoryBalanceRepository:GenericRepository<InventoryBalance>
    {
        public InventoryBalanceRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}