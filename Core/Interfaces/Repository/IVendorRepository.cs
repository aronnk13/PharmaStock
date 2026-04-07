using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{   
    public interface IVendorRepository : IGenericRepository<Vendor>
    {
        Task<Vendor?> GetByNameAsync(string name);
    }
}

