using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Core.Services
{
    public class QCODashboardService : IQCODashboardService
    {
        private readonly IQuarantineRepository _quarantineRepo;
        private readonly IRecallNoticeRepository _recallRepo;
        private readonly IStockAdjustmentRepository _adjustmentRepo;
        private readonly IExpiryWatchRepository _expiryRepo;

        public QCODashboardService(
            IQuarantineRepository quarantineRepo,
            IRecallNoticeRepository recallRepo,
            IStockAdjustmentRepository adjustmentRepo,
            IExpiryWatchRepository expiryRepo)
        {
            _quarantineRepo = quarantineRepo;
            _recallRepo = recallRepo;
            _adjustmentRepo = adjustmentRepo;
            _expiryRepo = expiryRepo;
        }

        public async Task<QCODashboardDTO> GetDashboardAsync()
        {
            var activeQuarantines = await _quarantineRepo.CountActiveAsync();
            var openRecalls = await _recallRepo.CountOpenAsync();
            var recentAdjustmentsCount = await _adjustmentRepo.CountRecentAsync(30);
            var nearExpiry = await _expiryRepo.GetNearExpiryAsync(30);
            var recentQuarantines = await _quarantineRepo.GetRecentAsync(5);
            var recentRecalls = await _recallRepo.GetRecentAsync(5);

            return new QCODashboardDTO
            {
                ActiveQuarantines = activeQuarantines,
                OpenRecalls = openRecalls,
                NearExpiryCount = nearExpiry.Count(),
                RecentAdjustmentsCount = recentAdjustmentsCount,
                RecentQuarantines = recentQuarantines.Select(q => new RecentQuarantineDTO
                {
                    QuarantaineActionId = q.QuarantaineActionId,
                    InventoryLotId = q.InventoryLotId,
                    ItemName = q.InventoryLot?.Item?.Drug?.GenericName,
                    BatchNumber = q.InventoryLot?.BatchNumber.ToString(),
                    Reason = q.Reason,
                    QuarantineDate = q.QuarantineDate,
                    Status = q.StatusNavigation?.Status
                             ?? q.Status switch { 1 => "Active", 2 => "Released", 3 => "Disposed", _ => "Unknown" }
                }).ToList(),
                RecentRecalls = recentRecalls.Select(r => new RecentRecallDTO
                {
                    RecallNoticeId = r.RecallNoticeId,
                    DrugName = r.Drug?.GenericName,
                    NoticeDate = r.NoticeDate,
                    Reason = r.Reason,
                    Action = r.ActionNavigation?.Action,
                    Status = r.Status
                }).ToList()
            };
        }
    }
}
