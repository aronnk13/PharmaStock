using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ColdChainLog
{
    public string LogId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? SensorId { get; set; }

    public DateTime Timestamp { get; set; }

    public decimal TemperatureC { get; set; }

    public string Status { get; set; } = null!;

    public virtual Location? Location { get; set; }
}
