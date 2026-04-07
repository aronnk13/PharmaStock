using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Drug
{
    public class CreateDrugDTO
    {
        [Required(ErrorMessage = "Generic Name is required")]
        [StringLength(200)]
        public string GenericName { get; set; } = null!;

        [StringLength(200)]
        public string? BrandName { get; set; }

        [Required(ErrorMessage = "Strength is required")]
        [StringLength(50)]
        public string Strength { get; set; } = null!;

        [Required(ErrorMessage = "Form ID is required")]
        public int Form { get; set; } // Foreign Key ID

        public string? Atccode { get; set; }

        [Required(ErrorMessage = "Control Class is required")]
        public int ControlClass { get; set; }

        [Required(ErrorMessage = "Storage Class is required")]
        public int StorageClass { get; set; }

        public bool Status { get; set; } = true; // Default to active
    }
}