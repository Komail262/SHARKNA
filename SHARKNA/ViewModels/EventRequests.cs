using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.ViewModels
{
    public class EventRequestViewModel
    {
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز السبب 500 حرف")]
        [DisplayName("سبب الرفض")]
        public string RejectionReasons { get; set; }

        public Guid EventId { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        [DisplayName("اسم الفعالية بالعربي")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الوصف 100 حرف")]
        [DisplayName("وصف الفعالية")]
        public string EventDescriptionAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الموضوع 100 حرف")]
        [DisplayName("موضوع الحدث بالعربي")]
        public string TopicAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الموقع 100 حرف")]
        [DisplayName("موقع الحدث بالعربي")]
        public string LocationAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [Range(1, int.MaxValue, ErrorMessage = "يرجى إدخال عدد صحيح أكبر من صفر")]
        [DisplayName("الحضور")]
        public int MaxAttendence { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم المتحدث 100 حرف")]
        [DisplayName("المتحدث بالعربي")]
        public string SpeakersAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DataType(DataType.DateTime, ErrorMessage = "يرجى إدخال تاريخ صحيح")]
        [DisplayName("تاريخ بداية الحدث")]
        public DateTime? EventStartDate { get; set; } // Nullable DateTime

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DataType(DataType.DateTime, ErrorMessage = "يرجى إدخال تاريخ صحيح")]
        [DisplayName("تاريخ نهاية الحدث")]
        public DateTime? EventEndtDate { get; set; } // Nullable DateTime

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("حالة الطلب")]
        public Guid RequestStatusId { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم الحالة 100 حرف")]
        [DisplayName("اسم حالة الطلب")]
        public string RequestStatusName { get; set; }

        [DisplayName("الجنس")]
        public bool? Gender { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اللجنة")]
        public Guid BoardId { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم اللجنة 100 حرف")]
        [DisplayName("اسم اللجنة")]
        public string BoardName { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الوصف 100 حرف")]
        [DisplayName("وصف اللجنة")]

        public bool? Genderr { get; set; }
        public string BoardDescriptionAr { get; set; }

        // الحقول الجديدة لمعلومات المستخدم
        public string CreatedByUserName { get; set; }
        public string CreatedByFullName { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedByMobileNumber { get; set; }
        public string CreatedByGender { get; set; } // يمكن استخدام string أو نوع enum للعرض
    }
}
