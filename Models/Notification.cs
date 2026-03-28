using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public int Category { get; set; }

    public int Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual NotificationCategory CategoryNavigation { get; set; } = null!;

    public virtual NotificationStatus StatusNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
