using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class NotificationCategory
{
    public int NotificationCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
