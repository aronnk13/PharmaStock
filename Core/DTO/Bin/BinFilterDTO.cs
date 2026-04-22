namespace PharmaStock.Core.DTO.Bin
{
    public class BinFilterDTO
    {
        public int? LocationId { get; set; }
        public int? StorageClassId { get; set; }
        public bool? IsQuarantine { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
