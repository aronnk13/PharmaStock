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

        public PharmacistDashboardService(
            IInventoryBalanceRepository balanceRepo,
            ITransferOrderRepository transferRepo,
            IDispenseRepository dispenseRepo)
        {
            _balanceRepo = balanceRepo;
            _transferRepo = transferRepo;
            _dispenseRepo = dispenseRepo;
        }

        public async Task<PharmacistDashboardDTO> GetDashboardAsync(int? locationId)
        {
            var balances = locationId.HasValue
                ? await _balanceRepo.GetByLocationAsync(locationId.Value)
                : await _balanceRepo.GetAllAsync();
            var totalStockItems = balances.Count();

            var allTransfers = await _transferRepo.GetAllAsync();
            var pendingIncoming = locationId.HasValue
                ? allTransfers.Count(t => t.ToLocationId == locationId.Value && t.Status == 1)
                : allTransfers.Count(t => t.Status == 1);

            var todayDispenses = locationId.HasValue
                ? await _dispenseRepo.CountTodayByLocationAsync(locationId.Value)
                : await _dispenseRepo.CountTodayAsync();
            var recentDispenses = locationId.HasValue
                ? await _dispenseRepo.GetRecentByLocationAsync(locationId.Value, 5)
                : await _dispenseRepo.GetRecentAsync(5);

            var recentTransfers = locationId.HasValue
                ? allTransfers.Where(t => t.ToLocationId == locationId.Value).OrderByDescending(t => t.CreatedDate).Take(5).ToList()
                : allTransfers.OrderByDescending(t => t.CreatedDate).Take(5).ToList();

            return new PharmacistDashboardDTO
            {
                TotalStockItems = totalStockItems,
                PendingIncomingTransfers = pendingIncoming,
                TodayDispenses = todayDispenses,
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
                    FromLocation = t.FromLocation?.Name,
                    CreatedDate = t.CreatedDate,
                    Status = t.StatusNavigation?.Status
                }).ToList()
            };
        }
    }
}
