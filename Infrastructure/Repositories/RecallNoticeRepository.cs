using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class RecallNoticeRepository : GenericRepository<RecallNotice>
    {
        public RecallNoticeRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}