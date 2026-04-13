namespace PharmaStock.Core.DTO.GRNItem
{
    public class GRNItemsPagedResponseDTO
    {
        public int TotalItems { get; set; }
        public List<GRNItemResponseDTO> Items { get; set; } = new();
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
