using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Drug
{
    public class UpdateDrugDTO
    {
        [Required(ErrorMessage = "DrugId is required for updates")]
        public int DrugId { get; set; }

        [Required(ErrorMessage = "Generic Name is required")]
        public string GenericName { get; set; } = null!;

        public string? BrandName { get; set; }

        [Required(ErrorMessage = "Strength is required")]
        public string Strength { get; set; } = null!;

        [Required(ErrorMessage = "Form ID is required")]
        public int Form { get; set; }

        public string? Atccode { get; set; }

        [Required(ErrorMessage = "Control Class is required")]
        public int ControlClass { get; set; }

        [Required(ErrorMessage = "Storage Class is required")]
        public int StorageClass { get; set; }

        [Required]
        public bool Status { get; set; }

    }
}