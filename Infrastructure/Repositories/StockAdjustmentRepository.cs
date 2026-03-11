using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class StockAdjustmentRepository : GenericRepository<StockAdjustment>
    {
        public StockAdjustmentRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}