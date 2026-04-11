namespace PharmaStock.Core.DTO.GRNItem
{
    public class GRNItemListDTO
    {
        public int GoodsReceiptItemId { get; set; }
        public int ItemId { get; set; }
        public string DrugName { get; set; } = null!;
        public string ControlClass { get; set; } = null!;
        public int BatchNumber { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int ReceivedQty { get; set; }
        public int AcceptedQty { get; set; }
        public int RejectedQty { get; set; }
        public string? Reason { get; set; }
    }

    public class GRNItemsPagedResponseDTO
    {
        public int GoodsReceiptId { get; set; }
        public string GrnStatus { get; set; } = null!;
        public int TotalItems { get; set; }
        public List<GRNItemListDTO> Items { get; set; } = new();
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
