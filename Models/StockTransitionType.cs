using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockTransitionType
{
    public int StockTransitionTypeId { get; set; }

    public string TransitionType { get; set; } = null!;

    public virtual ICollection<StockTransition> StockTransitions { get; set; } = new List<StockTransition>();
}
