using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class InventoryReport
{
    public string ReportId { get; set; } = null!;

    public string? Scope { get; set; }

    public string? Metrics { get; set; }

    public DateTime GeneratedDate { get; set; }
}
