using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class RecallNotice
{
    public int RecallNoticeId { get; set; }

    public int DrugId { get; set; }

    public DateOnly NoticeDate { get; set; }

    public string? Reason { get; set; }

    public int Action { get; set; }

    public bool Status { get; set; }

    public virtual RecallAction ActionNavigation { get; set; } = null!;

    public virtual Drug Drug { get; set; } = null!;
}
