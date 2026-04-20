namespace PharmaStock.Core.DTO.Item
{
    public class GetItemDTO
    {
        public int ItemId { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; } = null!;
        public int? PackSize { get; set; }
        public int UoMId { get; set; }
        public string UoMCode { get; set; } = null!;
        public decimal ConversionToEach { get; set; }
        public string? Barcode { get; set; }
        public bool Status { get; set; }
    }
}
