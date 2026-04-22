namespace PharmaStock.Core.DTO.Dashboard
{
    public class InventoryDashboardDTO
    {
        public int TotalInventoryLots { get; set; }
        public int NearExpiryItems { get; set; }
        public int ExpiredItems { get; set; }
        public int OpenTransferOrders { get; set; }
        public int PendingReplenishments { get; set; }
        public int ActiveExpiryWatches { get; set; }
        public int TotalLocations { get; set; }
        public int LowStockItems { get; set; }
        public List<RecentTransferDTO> RecentTransfers { get; set; } = new();
        public List<NearExpiryAlertDTO> NearExpiryAlerts { get; set; } = new();
    }

    public class RecentTransferDTO
    {
        public int TransferOrderId { get; set; }
        public string FromLocation { get; set; } = "";
        public string ToLocation { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = "";
    }

    public class NearExpiryAlertDTO
    {
        public int InventoryLotId { get; set; }
        public string ItemName { get; set; } = "";
        public int BatchNumber { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int DaysToExpire { get; set; }
    }
}
