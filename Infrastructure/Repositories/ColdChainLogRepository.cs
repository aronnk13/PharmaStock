using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ColdChainLogRepository : GenericRepository<ColdChainLog>, IColdChainLogRepository
    {
        public ColdChainLogRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<ColdChainLog>> GetAllWithDetailsAsync()
        {
            return await _pharmaStockContext.ColdChainLogs
                .Include(c => c.Location)
                .OrderByDescending(c => c.Timestamp)
                .ToListAsync();
        }

        public async Task<int> CountActiveExcursionsAsync()
        {
            return await _pharmaStockContext.ColdChainLogs
                .Where(c => c.Status == "Excursion")
                .CountAsync();
        }
    }
}
