using System;

namespace PharmaStock.Models;

public partial class ColdChainLog
{
    public int LogId { get; set; }

    public int LocationId { get; set; }

    public string SensorId { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public decimal TemperatureC { get; set; }

    public string Status { get; set; } = null!;  // "Normal" or "Excursion"

    public virtual Location Location { get; set; } = null!;
}
