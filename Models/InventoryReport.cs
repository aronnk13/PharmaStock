using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class InventoryReport
{
    public int InventoryReportId { get; set; }

    public int Scope { get; set; }

    public bool Metrics { get; set; }

    public DateTime GeneratedDate { get; set; }

    public virtual ReportScope ScopeNavigation { get; set; } = null!;
}
