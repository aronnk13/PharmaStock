using System;

namespace PharmaStock.Models;

public partial class PutAwayTask
{
    public int TaskId { get; set; }

    public int GrnItemId { get; set; }

    public int TargetBinId { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;  // "Pending" or "Completed"

    public virtual GoodsReceiptItem GrnItem { get; set; } = null!;

    public virtual Bin TargetBin { get; set; } = null!;
}
