using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces
{
    public interface IAuditLogService
    {
        Task<bool> CreateLogAsync(AuditDto dto);
    }
}