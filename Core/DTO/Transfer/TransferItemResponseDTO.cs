using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Transfer
{
    public class TransferItemResponseDTO
    {
        public int TransferItemId { get; set; }
        public int TransferOrderId { get; set; }
        public int ItemId { get; set; }
        public int InventoryLotId { get; set; }
        public int Quantity { get; set; }
    }
}