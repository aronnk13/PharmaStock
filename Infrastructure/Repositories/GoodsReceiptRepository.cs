using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class GoodsReceiptRepository:GenericRepository<GoodsReceipt>
    {
        public GoodsReceiptRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}