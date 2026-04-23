namespace PharmaStock.Core.DTO.QCO
{
    public class ColdChainLogDTO
    {
        public int LogId { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public string SensorId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public decimal TemperatureC { get; set; }
        public string Status { get; set; } = null!;
    }

    public class CreateColdChainLogDTO
    {
        public int LocationId { get; set; }
        public string SensorId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public decimal TemperatureC { get; set; }
        public string Status { get; set; } = null!;
    }
}
