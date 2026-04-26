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

        // Only show active, non-expired lots with stock on hand.
        // This mirrors the LotExpiryBackgroundService logic and covers the
        // timing gap when a lot expires between daily job runs.
        private static readonly Func<Models.InventoryBalance, bool> IsLive = b =>
            b.QuantityOnHand > 0 &&
            b.InventoryLot != null &&
            b.InventoryLot.Status == 1 &&
            b.InventoryLot.ExpiryDate > DateOnly.FromDateTime(DateTime.Today);

        public async Task<IEnumerable<InventoryBalanceDTO>> GetAllAsync()
        {
            var balances = await _repo.GetAllWithDetailsAsync();
            return balances.Where(IsLive).Select(Map);
        }

        public async Task<IEnumerable<InventoryBalanceDTO>> GetByLocationAsync(int locationId)
        {
            var balances = await _repo.GetByLocationAsync(locationId);
            return balances.Where(IsLive).Select(Map);
        }

        public async Task<IEnumerable<InventoryBalanceDTO>> GetByItemAsync(int itemId)
        {
            var balances = await _repo.GetByItemAsync(itemId);
            return balances.Where(IsLive).Select(Map);
        }

        public async Task<IEnumerable<InventoryBalanceDTO>> GetLowStockAsync(int threshold)
        {
            var balances = await _repo.GetLowStockAsync(threshold);
            return balances.Where(IsLive).Select(Map);
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
