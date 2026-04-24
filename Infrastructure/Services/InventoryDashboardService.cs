using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Dashboard;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Services
{
    public class InventoryDashboardService : IInventoryDashboardService
    {
        private readonly PharmaStockContext _context;

        public InventoryDashboardService(PharmaStockContext context)
        {
            _context = context;
        }

        public async Task<InventoryDashboardDTO> GetDashboardStatsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var in90Days = today.AddDays(90);
            var in30Days = today.AddDays(30);

            var totalLots = await _context.InventoryLots.CountAsync();
            var nearExpiry = await _context.InventoryLots
                .Where(l => l.ExpiryDate <= in90Days && l.ExpiryDate >= today)
                .CountAsync();
            var expired = await _context.InventoryLots
                .Where(l => l.ExpiryDate < today)
                .CountAsync();
            var openTransfers = await _context.TransferOrders
                .Where(t => t.Status == 1)
                .CountAsync();
            var pendingReplenishments = await _context.ReplenishmentRequests
                .Where(r => r.Status == 1)
                .CountAsync();
            var activeWatches = await _context.ExpiryWatches
                .Where(e => e.Status == true)
                .CountAsync();
            var totalLocations = await _context.Locations.CountAsync();
            var lowStock = await _context.InventoryBalances
                .Where(b => (b.QuantityOnHand - b.ReservedQty) <= 10)
                .CountAsync();

            var recentTransfers = await _context.TransferOrders
                .Include(t => t.FromLocation)
                .Include(t => t.ToLocation)
                .Include(t => t.StatusNavigation)
                .OrderByDescending(t => t.CreatedDate)
                .Take(5)
                .Select(t => new RecentTransferDTO
                {
                    TransferOrderId = t.TransferOrderId,
                    FromLocation = t.FromLocation.Name,
                    ToLocation = t.ToLocation.Name,
                    CreatedDate = t.CreatedDate,
                    Status = t.StatusNavigation.Status
                })
                .ToListAsync();

            var nearExpiryAlerts = await _context.InventoryLots
                .Include(l => l.Item).ThenInclude(i => i.Drug)
                .Where(l => l.ExpiryDate <= in30Days && l.ExpiryDate >= today)
                .OrderBy(l => l.ExpiryDate)
                .Take(5)
                .Select(l => new NearExpiryAlertDTO
                {
                    InventoryLotId = l.InventoryLotId,
                    ItemName = l.Item.Drug.GenericName,
                    BatchNumber = l.BatchNumber.ToString(),
                    ExpiryDate = l.ExpiryDate,
                    DaysToExpire = l.ExpiryDate.DayNumber - today.DayNumber
                })
                .ToListAsync();

            return new InventoryDashboardDTO
            {
                TotalInventoryLots = totalLots,
                NearExpiryItems = nearExpiry,
                ExpiredItems = expired,
                OpenTransferOrders = openTransfers,
                PendingReplenishments = pendingReplenishments,
                ActiveExpiryWatches = activeWatches,
                TotalLocations = totalLocations,
                LowStockItems = lowStock,
                RecentTransfers = recentTransfers,
                NearExpiryAlerts = nearExpiryAlerts
            };
        }
    }
}
