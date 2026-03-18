using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ItemRepository:GenericRepository<Item>
    {
        public ItemRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}