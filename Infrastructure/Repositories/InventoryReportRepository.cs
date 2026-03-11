using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class InventoryReportRepository:GenericRepository<InventoryReport>
    {
        public InventoryReportRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}