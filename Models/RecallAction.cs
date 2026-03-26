using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class RecallAction
{
    public int RecallActionId { get; set; }

    public string Action { get; set; } = null!;

    public virtual ICollection<RecallNotice> RecallNotices { get; set; } = new List<RecallNotice>();
}
