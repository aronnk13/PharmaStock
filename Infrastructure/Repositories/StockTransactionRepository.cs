using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class StockTransactionRepository : GenericRepository<StockTransaction>
    {
        public StockTransactionRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}