namespace SHARKNA.Models
{
    public class tblEvents
    {
        public Guid Id { get; set; }
        public string EventTitleAr { get; set; }
        public string EventTitleEn { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndtDate { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime EndRegTime { get; set; }
        public string SpeakersAr { get; set; }
        public string SpeakersEn { get; set; }
        public string TopicAr { get; set; }
        public string TopicEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string LocationAr { get; set; }
        public string LocationEn { get; set; }
        public int MaxAttendence { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        ICollection<tblEventRegistrations> EventReg { get; set; }
        ICollection<tblEventRequests> EventReq { get; set; }
        ICollection<tblEventAttendence> EventAttend { get; set; }

    }
}
