using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class PurchaseOrderRepository:GenericRepository<PurchaseOrder>
    {
        public PurchaseOrderRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}