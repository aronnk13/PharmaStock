using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class InventoryLotStatus
{
    public int InventoryLotStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
}
