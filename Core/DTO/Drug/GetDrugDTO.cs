using System;

namespace PharmaStock.Core.DTO.Drug
{
    public class GetDrugDTO
    {
        public int DrugId { get; set; }
        public string GenericName { get; set; }
        public string BrandName { get; set; }
        public string Strength { get; set; }
        public int Form { get; set; }
        public string Atccode { get; set; }
        public int ControlClass { get; set; }
        public int StorageClass { get; set; }
        public bool Status { get; set; }
    }
}
