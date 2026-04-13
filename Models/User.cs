using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedOn { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public bool StatusId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Audit> Audits { get; set; } = new List<Audit>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual UserRole Role { get; set; } = null!;

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockTransition> StockTransitions { get; set; } = new List<StockTransition>();
}
