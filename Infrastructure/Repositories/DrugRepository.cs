using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class DrugRepository:GenericRepository<Drug>
    {
        public DrugRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}