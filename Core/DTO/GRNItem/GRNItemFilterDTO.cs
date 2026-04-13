namespace PharmaStock.Core.DTO.GRNItem
{
    public class GRNItemFilterDTO
    {
        public int? BatchNumber { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
