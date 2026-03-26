using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class DispenseRef
{
    public int DispenseRefId { get; set; }

    public int LocationId { get; set; }

    public int ItemId { get; set; }

    public int InventoryLotId { get; set; }

    public int Quantity { get; set; }

    public DateTime DispenseDate { get; set; }

    public bool Status { get; set; }

    public int Destination { get; set; }

    public virtual DestinationType DestinationNavigation { get; set; } = null!;

    public virtual InventoryLot InventoryLot { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
