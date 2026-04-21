namespace PharmaStock.Core.DTO.Pharmacist
{
    public class PharmacistDashboardDTO
    {
        public int TotalStockItems { get; set; }
        public int PendingIncomingTransfers { get; set; }
        public int TodayDispenses { get; set; }
        public int NearExpiryAtLocation { get; set; }
        public List<RecentDispenseDTO> RecentDispenses { get; set; } = new();
        public List<IncomingTransferSummaryDTO> IncomingTransferSummary { get; set; } = new();
    }

    public class RecentDispenseDTO
    {
        public int DispenseRefId { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime DispenseDate { get; set; }
        public string? Destination { get; set; }
    }

    public class IncomingTransferSummaryDTO
    {
        public int TransferOrderId { get; set; }
        public string? FromLocation { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Status { get; set; }
    }
}
