using System;

namespace PharmaStock.Core.DTO.Drug
{
    public class DrugFilterDTO
    {
        public string? GenericName { get; set; }
        public int? StorageClass{ get; set; }
        public int? ControlClass { get; set; }
        public bool? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}