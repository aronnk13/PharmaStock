using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class NotificationStatus
{
    public int NotificationStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
