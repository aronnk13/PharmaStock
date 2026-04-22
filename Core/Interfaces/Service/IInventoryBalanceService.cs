using PharmaStock.Core.DTO.InventoryBalance;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IInventoryBalanceService
    {
        Task<IEnumerable<InventoryBalanceDTO>> GetAllAsync();
        Task<IEnumerable<InventoryBalanceDTO>> GetByLocationAsync(int locationId);
        Task<IEnumerable<InventoryBalanceDTO>> GetByItemAsync(int itemId);
        Task<IEnumerable<InventoryBalanceDTO>> GetLowStockAsync(int threshold);
    }
}
