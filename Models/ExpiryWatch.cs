using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ExpiryWatch
{
    public string WatchId { get; set; } = null!;

    public string? LotId { get; set; }

    public int DaysToExpire { get; set; }

    public DateTime FlagDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual InventoryLot? Lot { get; set; }
}
