using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ReplenishmentRequestRepository : GenericRepository<ReplenishmentRequest>
    {
        public ReplenishmentRequestRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}