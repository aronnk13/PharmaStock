namespace PharmaStock.Core.DTO.PurchaseOrder
{
    public class PurchaseOrderResponseDTO
    {
        public int PurchaseOrderId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly ExpectedDate { get; set; }
        public int PurchaseOrderStatusId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
