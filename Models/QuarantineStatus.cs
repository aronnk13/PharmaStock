using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class QuarantineStatus
{
    public int QuarantineStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<QuarantaineAction> QuarantaineActions { get; set; } = new List<QuarantaineAction>();
}
