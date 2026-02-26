using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Drug
{
    public string DrugId { get; set; } = null!;

    public string GenericName { get; set; } = null!;

    public string? BrandName { get; set; }

    public string? Strength { get; set; }

    public string? Form { get; set; }

    public string? Atccode { get; set; }

    public string? ControlClass { get; set; }

    public string? StorageClass { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<RecallNotice> RecallNotices { get; set; } = new List<RecallNotice>();
}
