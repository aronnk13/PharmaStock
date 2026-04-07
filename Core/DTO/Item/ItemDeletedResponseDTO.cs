using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pharmaStock.Core.DTO.Item
{
    public class ItemDeletedResponseDTO
    {
        public bool IsDeleted { get; set; }
        public string? Message { get; set; }
    }
}