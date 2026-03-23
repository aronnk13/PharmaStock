using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repository;

        public AuditLogService(IAuditLogRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateLogAsync(int userId, string action, string resource, string metadata)
        {
            var audit = new Audit
            {
                UserId = userId,
                Action = action == "CREATE" || action == "UPDATE" || action == "DELETE", 
                Resource = resource,
                EventTimestamp = DateTime.UtcNow,
                Metadata = metadata
            };

            await _repository.AddAsync(audit);
        }

        public async Task<IEnumerable<Audit>> GetAuditsAsync()
        {
            return await _repository.GetAllAsync();
        }

    }
}