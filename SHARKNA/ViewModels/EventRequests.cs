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

        public string EventName { get; set; } // new

        [DisplayName("حالة الطلب")]
        public Guid RequestStatusId { get; set; }

        public string RequestStatusName { get; set; } // new

        [DisplayName("اللجنة")]
        public Guid BoardId { get; set; }

        public string BoardName { get; set; } // new
    }
}
