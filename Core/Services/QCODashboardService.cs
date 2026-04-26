using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;


namespace PharmaStock.Core.Services
{
    public class QCODashboardService : IQCODashboardService
    {
        private readonly IQuarantineRepository _quarantineRepo;
        private readonly IStockAdjustmentRepository _adjustmentRepo;
        private readonly IAuditLogRepository _auditRepo;

        public QCODashboardService(
            IQuarantineRepository quarantineRepo,
            IStockAdjustmentRepository adjustmentRepo,
            IAuditLogRepository auditRepo)
        {
            _quarantineRepo = quarantineRepo;
            _adjustmentRepo = adjustmentRepo;
            _auditRepo = auditRepo;
        }

        public async Task<QCODashboardDTO> GetDashboardAsync()
        {
            int activeQuarantines = 0;
            int recentAdjustmentsCount = 0;
            IEnumerable<QuarantaineAction> recentQuarantines = Enumerable.Empty<QuarantaineAction>();
            int auditEventsToday = 0;

            try { activeQuarantines = await _quarantineRepo.CountActiveAsync(); } catch { }
            try { recentAdjustmentsCount = await _adjustmentRepo.CountRecentAsync(30); } catch { }
            try { recentQuarantines = await _quarantineRepo.GetRecentAsync(5); } catch { }
            try { auditEventsToday = await _auditRepo.CountTodayAsync(); } catch { }

            return new QCODashboardDTO
            {
                ActiveQuarantines = activeQuarantines,
                RecentAdjustmentsCount = recentAdjustmentsCount,
                AuditEventsToday = auditEventsToday,
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
                }).ToList()
            };
        }
    }
}
