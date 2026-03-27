using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int GoodsReceiptItemId { get; set; }

    public int TargetBinId { get; set; }

    public int Quantity { get; set; }

    public bool Status { get; set; }

    public virtual GoodsReceiptItem GoodsReceiptItem { get; set; } = null!;

    public virtual Bin TargetBin { get; set; } = null!;
}
