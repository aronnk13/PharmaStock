namespace PharmaStock.Core.DTO.GRNItem
{
    public class GRNItemFilterDTO
    {
        public int GoodsReceiptId { get; set; }
        public int? GoodsReceiptItemId { get; set; }
        public int? ItemId { get; set; }
        public int? BatchNumber { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
