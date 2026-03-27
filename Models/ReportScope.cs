using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ReportScope
{
    public int ReportScopeId { get; set; }

    public string Scope { get; set; } = null!;

    public virtual ICollection<InventoryReport> InventoryReports { get; set; } = new List<InventoryReport>();
}
