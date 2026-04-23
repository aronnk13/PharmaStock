using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IInventoryBalanceRepository : IGenericRepository<InventoryBalance>
    {
        Task<IEnumerable<InventoryBalance>> GetByLocationAsync(int locationId);
        Task<IEnumerable<InventoryBalance>> GetAllWithDetailsAsync();
        Task<IEnumerable<InventoryBalance>> GetByItemAsync(int itemId);
        Task<IEnumerable<InventoryBalance>> GetLowStockAsync(int threshold);
    }
}
