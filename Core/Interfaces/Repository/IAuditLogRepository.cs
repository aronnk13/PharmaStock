using System;
using PharmaStock.Models;
using PharmaStock.Core.Interfaces;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        
    }
}