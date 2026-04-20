namespace PharmaStock.Core.DTO.Drug
{
    public class GetDrugDTO
    {
        public int DrugId { get; set; }
        public string GenericName { get; set; } = null!;
        public string? BrandName { get; set; }
        public string Strength { get; set; } = null!;
        public int FormId { get; set; }
        public string FormName { get; set; } = null!;
        public string? Atccode { get; set; }
        public int ControlClassId { get; set; }
        public string ControlClassName { get; set; } = null!;
        public int StorageClassId { get; set; }
        public string StorageClassName { get; set; } = null!;
        public bool Status { get; set; }
    }
}
