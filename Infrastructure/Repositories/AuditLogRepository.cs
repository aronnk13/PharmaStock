using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Audit;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class AuditLogRepository(PharmaStockContext pharmaStockContext) : IAuditLogRepository
    {
        public async System.Threading.Tasks.Task<List<GetAuditDTO>> GetAllAsync()
        {
            return await pharmaStockContext.Audits
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new GetAuditDTO
                {
                    AuditId = a.AuditId,
                    UserId = a.UserId,
                    Username = a.User.Username,
                    Action = a.Action,
                    Resource = a.Resource,
                    Timestamp = a.Timestamp,
                    Metadata = a.Metadata
                })
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<int> CountTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            return await pharmaStockContext.Audits
                .Where(a => a.Timestamp >= today && a.Timestamp < tomorrow)
                .CountAsync();
        }

        public async Task<AuditLog> AddAsync(Audit log)
        {
            pharmaStockContext.Audits.Add(log);
            await pharmaStockContext.SaveChangesAsync();
            return new AuditLog { Result = true };
        }
    }
}