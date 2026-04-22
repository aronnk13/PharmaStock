namespace PharmaStock.Core.DTO.ExpiryWatch
{
    public class ExpiryWatchDTO
    {
        public int ExpiryWatchId { get; set; }
        public int InventoryLotId { get; set; }
        public int BatchNumber { get; set; }
        public string? ItemName { get; set; }
        public int ItemId { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int DaysToExpire { get; set; }   // actual days remaining from today
        public DateOnly FlagDate { get; set; }
        public bool Status { get; set; }
    }
}
