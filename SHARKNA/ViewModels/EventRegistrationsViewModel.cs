using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Models;

namespace SHARKNA.ViewModels
{
    public class EventRegistrationsViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("وقت التسجيل")]
        public DateTime RegDate { get; set; }

        [StringLength(100)]
        [DisplayName(" سبب الرفض")]
        public string RejectionReasons { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("رقم الجوال")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم بالعربي")]
        public string FullNameAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم بالانجليزي")]
        public string FullNameEn { get; set; }


       // public ICollection<tblEvents> EventsOfList { get; set; }

    //    public ICollection<tblRequestStatus> RequeststatusOfList { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("الحدث")]
        public Guid EventId { get; set; }


        [DisplayName("حالة الطلب")]
        public Guid RequestStatusId { get; set; }

    }
}
