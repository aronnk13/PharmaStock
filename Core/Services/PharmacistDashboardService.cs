using PharmaStock.Core.DTO.Pharmacist;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Core.Services
{
    public class PharmacistDashboardService : IPharmacistDashboardService
    {
        private readonly IInventoryBalanceRepository _balanceRepo;
        private readonly ITransferOrderRepository _transferRepo;
        private readonly IDispenseRepository _dispenseRepo;
        private readonly IExpiryWatchRepository _expiryRepo;

        public PharmacistDashboardService(
            IInventoryBalanceRepository balanceRepo,
            ITransferOrderRepository transferRepo,
            IDispenseRepository dispenseRepo,
            IExpiryWatchRepository expiryRepo)
        {
            _balanceRepo = balanceRepo;
            _transferRepo = transferRepo;
            _dispenseRepo = dispenseRepo;
            _expiryRepo = expiryRepo;
        }

        public async Task<PharmacistDashboardDTO> GetDashboardAsync(int locationId)
        {
            var balances = await _balanceRepo.GetByLocationAsync(locationId);
            var totalStockItems = balances.Count();

            var allTransfers = await _transferRepo.GetAllWithDetailsAsync();
            var pendingIncoming = allTransfers.Count(t => t.ToLocationId == locationId && t.Status == 1);

            var todayDispenses = await _dispenseRepo.CountTodayByLocationAsync(locationId);
            var recentDispenses = await _dispenseRepo.GetRecentByLocationAsync(locationId, 5);

            var nearExpiry = await _expiryRepo.GetNearExpiryAsync(30);
            var nearExpiryAtLocation = nearExpiry
                .Where(e => e.InventoryLot?.InventoryBalances.Any(b => b.LocationId == locationId) == true)
                .Count();

            var recentTransfers = allTransfers
                .Where(t => t.ToLocationId == locationId)
                .OrderByDescending(t => t.CreatedDate)
                .Take(5)
                .ToList();

            return new PharmacistDashboardDTO
            {
                TotalStockItems = totalStockItems,
                PendingIncomingTransfers = pendingIncoming,
                TodayDispenses = todayDispenses,
                NearExpiryAtLocation = nearExpiryAtLocation,
                RecentDispenses = recentDispenses.Select(d => new RecentDispenseDTO
                {
                    DispenseRefId = d.DispenseRefId,
                    ItemName = d.Item?.Drug?.GenericName,
                    Quantity = d.Quantity,
                    DispenseDate = d.DispenseDate,
                    Destination = d.DestinationNavigation?.Type
                }).ToList(),
                IncomingTransferSummary = recentTransfers.Select(t => new IncomingTransferSummaryDTO
                {
                    TransferOrderId = t.TransferOrderId,
                    FromLocation    = t.FromLocation?.Name,
                    ItemCount       = t.TransferItems?.Count ?? 0,
                    CreatedDate     = t.CreatedDate,
                    Status          = t.StatusNavigation?.Status
                }).ToList()
            };
        }
    }
}
