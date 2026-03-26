using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Drug
{
    public int DrugId { get; set; }

    public string GenericName { get; set; } = null!;

    public string? BrandName { get; set; }

    public string Strength { get; set; } = null!;

    public int Form { get; set; }

    public string? Atccode { get; set; }

    public int ControlClass { get; set; }

    public int StorageClass { get; set; }

    public bool Status { get; set; }

    public virtual ControlClass ControlClassNavigation { get; set; } = null!;

    public virtual DrugForm FormNavigation { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<RecallNotice> RecallNotices { get; set; } = new List<RecallNotice>();

    public virtual DrugStorageClass StorageClassNavigation { get; set; } = null!;
}
