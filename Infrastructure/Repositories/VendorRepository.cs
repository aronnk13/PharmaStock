using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class VendorRepository : GenericRepository<Vendor>
    {
        public VendorRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}