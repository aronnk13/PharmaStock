using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Item
{

    public class ItemDTO
    {
        public int DrugId { get; set; }
        public int? PackSize { get; set; }
        public int UoM { get; set; }
        public decimal ConversionToEach { get; set; }
        public string Barcode { get; set; } = null!;
        public bool Status { get; set; }
        public int ItemId { get; internal set; }
    }

}