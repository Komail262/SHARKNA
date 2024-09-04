using System.ComponentModel.DataAnnotations.Schema;

namespace SHARKNA.Models
{
    public class tblEvents
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EventTitleAr { get; set; }
        public string EventTitleEn { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndtDate { get; set; }
        public string SpeakersAr { get; set; }
        public string SpeakersEn { get; set; }
        public string TopicAr { get; set; }
        public string TopicEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string LocationAr { get; set; }
        public string LocationEn { get; set; }
        public int MaxAttendence { get; set; }
        public bool? Gender { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public tblBoards Board { get; set; }
        public Guid BoardId { get; set; }
        ICollection<tblEventRegistrations> EventReg { get; set; }
        ICollection<tblEventRequests> EventReq { get; set; }
        ICollection<tblEventAttendence> EventAttend { get; set; }

    }
}
