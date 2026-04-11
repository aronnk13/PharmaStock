namespace PharmaStock.Core.DTO.GRNItem
{
    public class GRNItemDeleteResponseDTO
    {
        public int GoodsReceiptItemId { get; set; }
        public string Status { get; set; } = "Inactive";
        public string Reason { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
