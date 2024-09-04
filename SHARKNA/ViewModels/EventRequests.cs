using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.ViewModels
{
    public class EventRequestViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("أسباب الرفض")]
        [StringLength(500)]
        public string RejectionReasons { get; set; }

        [DisplayName("الفعالية")]
        public Guid EventId { get; set; }

        public string EventName { get; set; }

        public string EventDescriptionAr { get; set; } // وصف الفعالية
        public string TopicAr { get; set; }
        public string LocationAr { get; set; }
        public int MaxAttendence { get; set; }

        public string SpeakersAr { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndtDate { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime EndRegTime { get; set; }

        [DisplayName("حالة الطلب")]
        public Guid RequestStatusId { get; set; }

        public string RequestStatusName { get; set; }

        [DisplayName("اللجنة")]
        public Guid BoardId { get; set; }

        public string BoardName { get; set; }

        public string BoardDescriptionAr { get; set; } // وصف اللجنة

    }
}
