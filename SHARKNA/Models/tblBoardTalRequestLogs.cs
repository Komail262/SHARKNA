namespace SHARKNA.Models
{
    public class tblBoardTalRequestLogs
    {
        public Guid Id { get; set; }
        public String OpType { get; set; }

        public DateTime OpDateTime { get; set; }
        public String CreatedBy { get; set; }
        public String AdditionalInfo { get; set; }

        public Guid ReqId { get; set; }

    }
}
