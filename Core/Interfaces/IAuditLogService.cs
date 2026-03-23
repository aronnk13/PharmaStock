using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces
{
    public interface IAuditLogService
    {
        Task CreateLogAsync(int userId, string action, string resource, string metadata);
    }
}