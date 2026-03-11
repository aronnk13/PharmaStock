using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class DispenseRefRepository:GenericRepository<DispenseRef>
    {
        public DispenseRefRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}