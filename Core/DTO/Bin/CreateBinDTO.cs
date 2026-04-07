namespace PharmaStock.Core.DTO.Bin
{
    public class CreateBinDTO
    {
        public int LocationId { get; set; }
        public string Code { get; set; } = null!;
        public int BinStorageClassId { get; set; }
        public bool IsQuarantine { get; set; }
        public int MaxCapacity { get; set; }
    }
}
