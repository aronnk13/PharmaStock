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

            var totalLots = await _context.InventoryLots.CountAsync();

            // Fetch in memory to avoid DateOnly SQL translation issues
            var allLots = await _context.InventoryLots.Select(l => l.ExpiryDate).ToListAsync();
            var expired = allLots.Count(d => d < today);

            var openTransfers = await _context.TransferOrders
                .Where(t => t.Status == 1)
                .CountAsync();
            var pendingReplenishments = await _context.ReplenishmentRequests
                .Where(r => r.Status == 1)
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

            return new InventoryDashboardDTO
            {
                TotalInventoryLots = totalLots,
                ExpiredItems = expired,
                OpenTransferOrders = openTransfers,
                PendingReplenishments = pendingReplenishments,
                TotalLocations = totalLocations,
                LowStockItems = lowStock,
                RecentTransfers = recentTransfers
            };
        }
    }
}
