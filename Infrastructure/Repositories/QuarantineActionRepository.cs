using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class QuarantineActionRepository : GenericRepository<QuarantineAction>
    {
        public QuarantineActionRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}