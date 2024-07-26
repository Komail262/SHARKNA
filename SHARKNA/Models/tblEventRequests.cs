namespace SHARKNA.Models // ahmed albahrani
{
    public class tblEventRequests
    {
        public Guid Id { get; set; }
        public string RejectionReasons { get; set; }
        public tblEvents Event { get; set; }
        public Guid EventId { get; set; }
        public tblRequestStatus RequestStatus { get; set; }
        public Guid RequestStatustId { get; set; }
        public tblBoards Board { get; set; }
        public Guid BoardId { get; set; }

    }
}
