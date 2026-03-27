using System;

namespace PharmaStock.Core.DTO.Drug
{
    public class GetDrugDTO
    {
        //public int Id { get; set; }
        public string? Atccode { get; set; }
        public string? BrandName { get; set; }
        public int ControlClass { get; set; }
        public int StorageClass { get; set; }
        public bool Status { get; set; }
    }
}