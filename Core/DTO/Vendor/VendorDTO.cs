using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Vendor
{
    public class VendorDTO
    {
      
        public int VendorId { get; set; }

        
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string ContactInfo { get; set; } = null!;

        public string? PaymentTerms { get; set; }

        public int? Rating { get; set; }

       
        public bool StatusId { get; set; } = true;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}