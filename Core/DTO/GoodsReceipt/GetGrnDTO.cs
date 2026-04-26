namespace PharmaStock.Core.DTO.GoodsReceipt
{
    public class GetGrnDTO
    {
        public int GoodsReceiptId { get; set; }
        public int PurchaseOrderId { get; set; }
        public string? VendorName { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; } = null!;
        public string? ReceivedBy { get; set; }
        public int ItemCount { get; set; }
    }

    public class GrnItemDetailDTO
    {
        public int GoodsReceiptItemId { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? BatchNumber { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int OrderedQty { get; set; }
        public int ReceivedQty { get; set; }
        public int AcceptedQty { get; set; }
        public int RejectedQty { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class GetGrnWithItemsDTO : GetGrnDTO
    {
        public string? LocationName { get; set; }
        public List<GrnItemDetailDTO> Items { get; set; } = new();
    }

    public class CompleteQcItemDTO
    {
        public int GrnItemId { get; set; }
        public int AcceptedQty { get; set; }
        public int RejectedQty { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class CompleteQcDTO
    {
        public List<CompleteQcItemDTO> Items { get; set; } = new();
    }

    public class CompleteQcResultDTO
    {
        public int GrnId { get; set; }
        public string GrnStatus { get; set; } = null!;
        public string PoStatus { get; set; } = null!;
        public int InventoryLotsCreated { get; set; }
        public int TotalAccepted { get; set; }
        public int TotalRejected { get; set; }
        public int QuarantineCreated { get; set; }
    }

    public class ApprovedPendingGrnDTO
    {
        public int PurchaseOrderId { get; set; }
        public string? VendorName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly ExpectedDate { get; set; }
        public string? Status { get; set; }
        public int ItemCount { get; set; }
    }

    public class PoItemDetailDTO
    {
        public int PurchaseItemId { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int OrderedQty { get; set; }
        public int AcceptedQty { get; set; }       // total accepted across all previous GRNs
        public int OutstandingQty { get; set; }    // OrderedQty - AcceptedQty
        public decimal UnitPrice { get; set; }
        public decimal TaxPct { get; set; }
    }

    public class PoWithItemsDTO
    {
        public int PurchaseOrderId { get; set; }
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly ExpectedDate { get; set; }
        public string? Status { get; set; }
        public List<PoItemDetailDTO> Items { get; set; } = new();
    }
}
