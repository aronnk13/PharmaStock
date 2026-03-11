using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ExpiryWatchRepository:GenericRepository<ExpiryWatch>
    {
        public ExpiryWatchRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}