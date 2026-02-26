using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class AuditLog
{
    public string AuditId { get; set; } = null!;

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? Resource { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Metadata { get; set; }

    public virtual User? User { get; set; }
}
