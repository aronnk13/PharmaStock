using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Item
{

    public class CreateItemDTO
    {
        public int DrugId { get; set; }
        public int? PackSize { get; set; }
        public int UoM { get; set; }
        public decimal ConversionToEach { get; set; }
        public string Barcode { get; set; } = null!;
        public bool Status { get; set; }
    }

}