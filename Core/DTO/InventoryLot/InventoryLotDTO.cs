using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.InventoryLot
{
  
    public class InventoryLotDTO
    {
        public int InventoryLotId { get; set; }

        public int ItemId { get; set; }

        public int BatchNumber { get; set; }

        public DateOnly ExpiryDate { get; set; }

        public int? ManufacturerId { get; set; }

        public int Status { get; set; }
    }
}