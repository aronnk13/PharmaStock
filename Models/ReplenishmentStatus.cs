using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ReplenishmentStatus
{
    public int ReplenishmentStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ReplenishmentRequest> ReplenishmentRequests { get; set; } = new List<ReplenishmentRequest>();
}
