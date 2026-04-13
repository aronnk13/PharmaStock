using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Bin
{
    public int BinId { get; set; }

    public int LocationId { get; set; }

    public string Code { get; set; } = null!;

    public int BinStorageClass { get; set; }

    public bool IsQuarantine { get; set; }

    public int MaxCapacity { get; set; }

    public bool StatusId { get; set; }

    public virtual BinStorageClass BinStorageClassNavigation { get; set; } = null!;

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual ICollection<StockTransition> StockTransitions { get; set; } = new List<StockTransition>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
