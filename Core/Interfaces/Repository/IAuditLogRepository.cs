using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Audit;
using PharmaStock.Models;
using Task = System.Threading.Tasks.Task;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IAuditLogRepository
    {
        Task<AuditLog> AddAsync(Audit log);
        Task<List<GetAuditDTO>> GetAllAsync();
        Task<int> CountTodayAsync();
    }
}