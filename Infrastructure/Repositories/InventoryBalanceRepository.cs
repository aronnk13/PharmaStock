using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class InventoryBalanceRepository : GenericRepository<InventoryBalance>, IInventoryBalanceRepository
    {
        public InventoryBalanceRepository(PharmaStockContext context) : base(context) { }

        public async Task<IEnumerable<InventoryBalance>> GetByLocationAsync(int locationId)
        {
            return await _pharmaStockContext.InventoryBalances
                .Include(b => b.Location)
                .Include(b => b.Bin)
                .Include(b => b.Item)
                .Include(b => b.InventoryLot)
                .Where(b => b.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryBalance>> GetByItemAsync(int itemId)
        {
            return await _pharmaStockContext.InventoryBalances
                .Include(b => b.Location)
                .Include(b => b.Bin)
                .Include(b => b.Item)
                .Include(b => b.InventoryLot)
                .Where(b => b.ItemId == itemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryBalance>> GetLowStockAsync(int threshold)
        {
            return await _pharmaStockContext.InventoryBalances
                .Include(b => b.Location)
                .Include(b => b.Bin)
                .Include(b => b.Item)
                .Include(b => b.InventoryLot)
                .Where(b => (b.QuantityOnHand - b.ReservedQty) <= threshold)
                .ToListAsync();
        }
    }
}
