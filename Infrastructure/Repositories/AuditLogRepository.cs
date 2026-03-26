using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class AuditLogRepository(PharmaStockContext pharmaStockContext) : IAuditLogRepository
    {
        public async Task<bool> AddAsync(Audit log)
        {
            try
            {
                pharmaStockContext.Audits.Add(log);
                await pharmaStockContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database update failed: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }
        }
    }
}