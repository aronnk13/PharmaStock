using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class QuarantineAction
{
    public string Qaid { get; set; } = null!;

    public string? LotId { get; set; }

    public DateTime QuarantineDate { get; set; }

    public string? Reason { get; set; }

    public DateTime? ReleasedDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual InventoryLot? Lot { get; set; }
}
