namespace PharmaStock.Core.DTO.QCO
{
    public class RecallNoticeDTO
    {
        public int RecallNoticeId { get; set; }
        public int DrugId { get; set; }
        public string? DrugName { get; set; }
        public DateOnly NoticeDate { get; set; }
        public string? Reason { get; set; }
        public int Action { get; set; }
        public string? ActionName { get; set; }
        public bool Status { get; set; }
    }
}
