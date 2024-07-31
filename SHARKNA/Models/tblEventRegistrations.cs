namespace SHARKNA.Models
{
    public class tblEventRegistrations
    {
        public Guid Id { get; set; }
        public DateTime RegDate { get; set; }
        public string RejectionReasons { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public tblEvents Events { get; set; }
        public Guid EventstId { get; set; }
        public tblRequestStatus EventStatus { get; set; }
        public Guid EvenetStatusId { get; set; }



    }
}
