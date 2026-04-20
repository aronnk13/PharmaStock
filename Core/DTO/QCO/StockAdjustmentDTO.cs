namespace PharmaStock.Core.DTO.QCO
{
    public class StockAdjustmentDTO
    {
        public int StockAdjustmentId { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int InventoryLotId { get; set; }
        public int QuantityDelta { get; set; }
        public int ReasonCode { get; set; }
        public string? ReasonDescription { get; set; }
        public int ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
