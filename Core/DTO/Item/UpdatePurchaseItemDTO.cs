using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Item
{
    public class UpdatePurchaseItemDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Ordered Quantity must be greater than zero")]
        public int OrderedQty { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than zero")]
        public decimal UnitPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Tax Percentage must be between 0 and 100")]
        public decimal TaxPct { get; set; }
    }
}