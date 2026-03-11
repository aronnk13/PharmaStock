using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class TransferItemRepository : GenericRepository<TransferItem>
    {
        public TransferItemRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}