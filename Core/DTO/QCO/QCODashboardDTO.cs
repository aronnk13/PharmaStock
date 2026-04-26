namespace PharmaStock.Core.DTO.QCO
{
    public class QCODashboardDTO
    {
        public int ActiveQuarantines { get; set; }
        public int RecentAdjustmentsCount { get; set; }
        public int AuditEventsToday { get; set; }
        public List<RecentQuarantineDTO> RecentQuarantines { get; set; } = new();
    }

    public class RecentQuarantineDTO
    {
        public int QuarantaineActionId { get; set; }
        public int InventoryLotId { get; set; }
        public string? ItemName { get; set; }
        public string? BatchNumber { get; set; }
        public string? Reason { get; set; }
        public DateTime QuarantineDate { get; set; }
        public string? Status { get; set; }
    }
}
