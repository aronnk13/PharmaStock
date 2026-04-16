using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pharmaStock.Core.DTO.Item
{
    public class ItemFilterDTO
    {
        public int? PackSize { get; set; }
        public bool? IsActive { get; set; }
    }
}