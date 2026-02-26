using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class RecallNotice
{
    public string RecallId { get; set; } = null!;

    public string? DrugId { get; set; }

    public DateTime NoticeDate { get; set; }

    public string? Reason { get; set; }

    public string? Action { get; set; }

    public string Status { get; set; } = null!;

    public virtual Drug? Drug { get; set; }
}
