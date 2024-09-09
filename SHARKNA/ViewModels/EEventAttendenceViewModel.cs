using SHARKNA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SHARKNA.ViewModels
{
    public class EEventAttendenceViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("تاريخ الحدث")]
        public DateTime EventDate { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("عدد أيام الفعالية")]
        public int Day { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم الفعالية")]
        public Guid EventsId { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم الحدث")]
        public string EventName { get; set; }  //هنا عشان يعرض اسم الفعالية




        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم الأعضاء بالفعالية")]
        public Guid EventsRegId { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("البريد الإلكتروني")]
        public string EEmail { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("وقت التسجيل")]
        public DateTime RRegDate { get; set; }
        

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("الإسم بالعربي")]
        public string NameAr { get; set; }


        public bool IsAttend { get; set; }

    }
}