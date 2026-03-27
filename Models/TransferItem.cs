using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class TransferItem
{
    public int TransferItemId { get; set; }

    public int TransferOrderId { get; set; }

    public int ItemId { get; set; }

    public int InventoryLotId { get; set; }

    public int Quantity { get; set; }

    public virtual InventoryLot InventoryLot { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual TransferOrder TransferOrder { get; set; } = null!;
}
