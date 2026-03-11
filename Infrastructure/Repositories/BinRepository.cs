using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class BinRepository:GenericRepository<Bin>
    {
        public BinRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}