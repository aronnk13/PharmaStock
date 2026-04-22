namespace PharmaStock.Core.DTO.Audit
{
    public class GetAuditDTO
    {
        public int AuditId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string Resource { get; set; } = null!;
        public DateTime? Timestamp { get; set; }
        public string? Metadata { get; set; }
    }
}
