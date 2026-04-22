using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Transfer
{
    public class TransferOrderResponseDTO
    {
        public int TransferOrderId { get; set; }
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }
}