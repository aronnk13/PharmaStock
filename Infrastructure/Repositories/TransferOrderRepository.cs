using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class TransferOrderRepository : GenericRepository<TransferOrder>, ITransferOrderRepository
    {
        private readonly PharmaStockContext _context;

        public TransferOrderRepository(PharmaStockContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> IsLocationValidAsync(int locationId) =>
            _context.Locations.AnyAsync(l => l.LocationId == locationId);

        public Task<bool> IsItemValidAsync(int itemId) =>
            _context.Items.AnyAsync(i => i.ItemId == itemId);

        public Task<bool> IsInventoryLotValidAsync(int inventoryLotId) =>
            _context.InventoryLots.AnyAsync(il => il.InventoryLotId == inventoryLotId);

        public Task<bool> IsTransferOrderValidAsync(int transferOrderId) =>
            _context.TransferOrders.AnyAsync(t => t.TransferOrderId == transferOrderId);

        public async Task<IEnumerable<TransferItem>> GetItemsByTransferOrderIdAsync(int transferOrderId) =>
            await _context.TransferItems.Where(t => t.TransferOrderId == transferOrderId).ToListAsync();
    }
}