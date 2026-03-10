using System;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class AuditLogRepository : GenericRepository<AuditLog>
    {
        public AuditLogRepository(PharmaStockContext context) : base(context)
        {
        }
    }
}