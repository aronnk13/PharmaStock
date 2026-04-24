namespace PharmaStock.Core.DTO.GRNItem
{
    public class GRNItemResponseDTO
    {

        public int GoodsReceiptItemId { get; set; }
        public int GoodsReceiptId { get; set; }
        public int PurchaseOrderItemId { get; set; }
        public int BatchNumber { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int ReceivedQty { get; set; }
        public int AcceptedQty { get; set; }
        public int RejectedQty { get; set; }
        public string? Reason { get; set; }
        public bool OverShipmentFlag { get; set; }
    }
}
