using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class QuarantaineAction
{
    public int QuarantaineActionId { get; set; }

    public int InventoryLotId { get; set; }

    public DateTime QuarantineDate { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime? ReleasedDate { get; set; }

    public int Status { get; set; }

    public virtual InventoryLot InventoryLot { get; set; } = null!;

    public virtual QuarantineStatus StatusNavigation { get; set; } = null!;
}
