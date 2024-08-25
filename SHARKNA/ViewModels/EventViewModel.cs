﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("تاريخ بداية الحدث")]
        public DateTime EventStartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("تاريخ نهاية الحدث")]
        public DateTime EventEndtDate { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName(" بداية الساعة الحدث")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DataType(DataType.DateTime)]
        [DisplayName(" نهاية الساعة الحدث")]
        public DateTime EndRegTime { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("المتحدث بي الحدث")]
        public string SpeakersAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("المتحدث بي الحدث بالانجليزي")]
        public string SpeakersEn { get;set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الموضوع الحدث بالعربي")]
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
        [DisplayName("موقع الحدث بالعرب")]
        public string LocationAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("موقع الحدث بالانجليزي")]

        public string LocationEn { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("الحد الأقصى للحضور")]
        public int MaxAttendence { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }



    }
}
