using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class InventoryLotRepository:GenericRepository<InventoryLot>
    {
        public InventoryLotRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}