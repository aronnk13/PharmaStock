using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Bin
{
    public string BinId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string Code { get; set; } = null!;

    public string? StorageClass { get; set; }

    public bool IsQuarantine { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<PutAwayTask> PutAwayTasks { get; set; } = new List<PutAwayTask>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
