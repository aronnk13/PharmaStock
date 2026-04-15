namespace PharmaStock.Core.DTO.GoodsReceipt
{
    public class GetGrnDTO
    {
        public int GoodsReceiptId { get; set; }
        public int PurchaseOrderId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; } = null!;
        public string? ReceivedBy { get; set; }
    }
}
