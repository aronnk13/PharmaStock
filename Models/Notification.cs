using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Notification
{
    public string NotificationId { get; set; } = null!;

    public int? UserId { get; set; }

    public string Message { get; set; } = null!;

    public string? Category { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual User? User { get; set; }
}
