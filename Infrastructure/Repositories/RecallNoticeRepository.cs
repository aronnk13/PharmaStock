using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class RecallNoticeRepository : GenericRepository<RecallNotice>, IRecallNoticeRepository
    {
        public RecallNoticeRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<RecallNotice>> GetAllWithDetailsAsync()
        {
            return await _pharmaStockContext.RecallNotices
                .Include(r => r.Drug)
                .Include(r => r.ActionNavigation)
                .OrderByDescending(r => r.NoticeDate)
                .ToListAsync();
        }

        public async Task<int> CountOpenAsync()
        {
            return await _pharmaStockContext.RecallNotices
                .Where(r => r.Status == false)
                .CountAsync();
        }

        public async Task<IEnumerable<RecallNotice>> GetRecentAsync(int count)
        {
            return await _pharmaStockContext.RecallNotices
                .Include(r => r.Drug)
                .Include(r => r.ActionNavigation)
                .OrderByDescending(r => r.NoticeDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
