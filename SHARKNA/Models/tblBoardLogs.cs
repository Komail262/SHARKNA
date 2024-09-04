namespace SHARKNA.Models
{
    public class tblBoardLogs
    {
        public Guid Id { get; set; }
        public string OpType { get; set; }
        public DateTime OpDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedTo { get; set; }
        public string AdditionalInfo { get; set; }
        public Guid BrdId { get; set; }
    }
}