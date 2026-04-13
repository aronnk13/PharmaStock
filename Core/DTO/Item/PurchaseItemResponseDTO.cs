using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Item
{
    public class PurchaseItemResponseDTO
    {
        public int PurchaseItemId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ItemId { get; set; }
        public int OrderedQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxPct { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}