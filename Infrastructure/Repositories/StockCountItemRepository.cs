using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class StockCountItemRepository : GenericRepository<StockCountItem>
    {
        public StockCountItemRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}