using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class StockCountRepository : GenericRepository<StockCount>
    {
        public StockCountRepository(PharmaStockContext context) : base(context)
        {
            
        }
            
    }
    
}
