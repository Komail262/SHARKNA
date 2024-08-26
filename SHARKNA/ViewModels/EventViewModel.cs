using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.ViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName(" اسم الحدث بالعربي")]
        public string EventTitleAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم الحدث بالانجليزي")]
        public string EventTitleEn { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DataType(DataType.DateTime, ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("تاريخ بداية الحدث")]
        public DateTime EventStartDate { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DataType(DataType.DateTime, ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("تاريخ نهاية الحدث")]
        public DateTime EventEndtDate { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("المتحدث بي الحدث")]
        public string SpeakersAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("المتحدث بي الحدث بالانجليزي")]
        public string SpeakersEn { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("موضوع الحدث بالعربي")]
        public string TopicAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الموضوع بالانجليزي")]
        public string TopicEn { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("وصف الحدث بالعربي")]
        public string DescriptionAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("وصف الحدث بالانجليزي")]
        public string DescriptionEn { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("موقع الحدث بالعربي")]
        public string LocationAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("موقع الحدث بالانجليزي")]
        public string LocationEn { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("الحد الأقصى للحضور")]
        public int MaxAttendence { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اللجنة")]
        public Guid BoardId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
