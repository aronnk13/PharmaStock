using System;
using PharmaStock.Models;


namespace PharmaStock.Infrastructure.Repositories
{
    public class PutAwayTaskRepository : GenericRepository<PutAwayTask>
    {
        public PutAwayTaskRepository(PharmaStockContext context) : base(context)
        {
            
        }
    }
}