using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Audit;
using PharmaStock.Models;
using Task = System.Threading.Tasks.Task;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IAuditLogService
    {
        Task<AuditLog> CreateLogAsync(AuditDto dto);
        Task<List<GetAuditDTO>> GetAllAsync();
    }
}