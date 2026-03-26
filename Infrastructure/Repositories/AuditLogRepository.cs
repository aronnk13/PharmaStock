using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private static readonly List<Audit> _logs = new();

        public async Task AddAsync(Audit log)
        {
            _logs.Add(log);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Audit>> GetAllAsync()
        {
            return await Task.FromResult(_logs);
        }
    }
}