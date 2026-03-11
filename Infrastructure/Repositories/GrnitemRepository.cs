using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class GrnitemRepository:GenericRepository<Grnitem>
    {
        public GrnitemRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}