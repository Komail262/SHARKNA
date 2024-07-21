namespace SHARKNA.Models
{
    public class tblBoardRequests
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; } //test
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }

        public tblBoards Board { get; set; }
        public Guid BoardId { get; set; }
        public tblRequestStatus RequestStatus { get; set; }
        public Guid RequestStatusId { get; set; }
        public string? RejectionReasons { get; set; }

    }
}
