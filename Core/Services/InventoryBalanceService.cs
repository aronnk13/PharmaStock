using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.InventoryBalance;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Core.Services
{
    public class InventoryBalanceService : IInventoryBalanceService
    {
        private readonly IInventoryBalanceRepository _repo;

        public InventoryBalanceService(IInventoryBalanceRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<InventoryBalanceDTO>> GetAllAsync()
        {
            var balances = await _repo.GetAllAsync();
            return balances.Select(Map);
        }

        public async Task<IEnumerable<InventoryBalanceDTO>> GetByLocationAsync(int locationId)
        {
            var balances = await _repo.GetByLocationAsync(locationId);
            return balances.Select(Map);
        }

        public async Task<IEnumerable<InventoryBalanceDTO>> GetByItemAsync(int itemId)
        {
            var balances = await _repo.GetByItemAsync(itemId);
            return balances.Select(Map);
        }

        public async Task<IEnumerable<InventoryBalanceDTO>> GetLowStockAsync(int threshold)
        {
            var balances = await _repo.GetLowStockAsync(threshold);
            return balances.Select(Map);
        }

        private static InventoryBalanceDTO Map(Models.InventoryBalance b) => new()
        {
            InventoryBalanceId = b.InventoryBalanceId,
            LocationId = b.LocationId,
            LocationName = b.Location?.Name,
            BinId = b.BinId,
            BinCode = b.Bin?.Code,
            ItemId = b.ItemId,
            ItemName = b.Item?.Drug?.GenericName,
            InventoryLotId = b.InventoryLotId,
            BatchNumber = b.InventoryLot?.BatchNumber.ToString(),
            ExpiryDate = b.InventoryLot?.ExpiryDate,
            QuantityOnHand = b.QuantityOnHand,
            ReservedQty = b.ReservedQty
        };
    }
}
