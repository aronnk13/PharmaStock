using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Transfer
{
    public class CreateTransferOrderDTO
    {
        [Required]
        public int FromLocationId { get; set; }
        [Required]
        public int ToLocationId { get; set; }
    }
}