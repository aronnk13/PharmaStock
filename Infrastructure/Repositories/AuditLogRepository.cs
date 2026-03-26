using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Audit;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class AuditLogRepository(PharmaStockContext pharmaStockContext) : IAuditLogRepository
    {
        public async Task<AuditLog> AddAsync(Audit log)
        {
            var res=new AuditLog();
            try
            {
                pharmaStockContext.Audits.Add(log);
                await pharmaStockContext.SaveChangesAsync();
                res.Result=true;
                res.Message=null;
            }
            catch (Exception ex)
            {
                res.Result=false;
                res.Message=$"Unexpected error: {ex.Message}";
            }
            return res;
        }
    }
}