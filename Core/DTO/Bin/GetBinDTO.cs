namespace PharmaStock.Core.DTO.Bin
{
    public class GetBinDTO
    {
        public int BinId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int BinStorageClassId { get; set; }
        public string StorageClass { get; set; } = null!;
        public bool IsQuarantine { get; set; }
        public int MaxCapacity { get; set; }
        public bool IsActive { get; set; }
    }
}
