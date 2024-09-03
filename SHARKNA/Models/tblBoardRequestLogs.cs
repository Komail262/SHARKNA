namespace SHARKNA.Models
{
    public class tblBoardRequestLogs
    {
        public Guid Id { get; set; }

        public Guid ReqId { get; set; }

        public string OpType { get; set; }

        public DateTime OpDateTime { get; set; }

        public string CreatedBy { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
