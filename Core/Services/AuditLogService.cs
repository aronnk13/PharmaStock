using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Audit;
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

        public async Task<List<GetAuditDTO>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<AuditLog> CreateLogAsync(AuditDto dto)
        {
            var audit = new Audit
            {
                UserId = dto.UserId,
                Action = dto.Action, 
                Resource = dto.Resource,
                Timestamp = DateTime.UtcNow,
                Metadata = dto.Metadata
            };
            return await _repository.AddAsync(audit);
        }
    }
}