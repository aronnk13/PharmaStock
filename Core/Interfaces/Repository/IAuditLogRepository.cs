using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Audit;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<AuditLog> AddAsync(Audit log);
    }
}