using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ExpiryWatch
{
    public int ExpiryWatchId { get; set; }

    public int InventoryLotId { get; set; }

    public int DaysToExpire { get; set; }

    public DateOnly FlagDate { get; set; }

    public bool Status { get; set; }

    public virtual InventoryLot InventoryLot { get; set; } = null!;
}
