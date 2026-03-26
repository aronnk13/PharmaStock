using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Auth
{
    public class DrugDeletedResponseDTO
    {
        public bool IsDeleted { get; set; }
        public string? Message { get; set; }
    }
}