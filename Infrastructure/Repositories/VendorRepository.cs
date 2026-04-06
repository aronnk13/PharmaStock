using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;




namespace PharmaStock.Infrastructure.Repositories
{
    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(PharmaStockContext context) : base(context) { }

        public async Task<Vendor?> GetByNameAsync(string name)
        {
            return await _pharmaStockContext.Set<Vendor>()
                .FirstOrDefaultAsync(v => v.Name == name);
        }
    }
}
