using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class PutAwayTask
{
    public string TaskId { get; set; } = null!;

    public string? GrnitemId { get; set; }

    public string? TargetBinId { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;

    public virtual Grnitem? Grnitem { get; set; }

    public virtual Bin? TargetBin { get; set; }
}
