using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IStockAdjustmentRepository : IGenericRepository<StockAdjustment>
    {
        Task<IEnumerable<StockAdjustment>> GetAllWithDetailsAsync();
        Task<int> CountRecentAsync(int days);
    }
}
