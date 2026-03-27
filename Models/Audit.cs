using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace PharmaStock.Models;

public partial class Audit
{
    public int AuditId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string Resource { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public string? Metadata { get; set; }

    public virtual User User { get; set; } = null!;
    [NotMapped]
    public object EventTimestamp { get; internal set; }
}
