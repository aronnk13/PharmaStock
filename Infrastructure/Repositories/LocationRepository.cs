using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class LocationRepository:GenericRepository<Location>
    {
        public LocationRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}