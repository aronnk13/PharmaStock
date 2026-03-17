using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class UoMRepository : GenericRepository<UoM>
    {
        public UoMRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}