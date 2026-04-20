namespace PharmaStock.Core.DTO.PurchaseOrder
{
    public class UpdatePurchaseOrderDTO
    {
        public DateOnly? ExpectedDate { get; set; }
        public int? PurchaseOrderStatusId { get; set; }
    }
}
