namespace PharmaStock.Core.DTO.Drug
{
    public class GetDrugDTO
    {
        public int DrugId { get; set; }
        public string GenericName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Strength { get; set; } = string.Empty;
        public string? Atccode { get; set; }

        // Form
        public int FormId { get; set; }
        public string? FormName { get; set; }

        // Control Class
        public int ControlClassId { get; set; }
        public string? ControlClassName { get; set; }

        // Storage Class
        public int StorageClassId { get; set; }
        public string? StorageClassName { get; set; }

        public bool Status { get; set; }
    }
}
