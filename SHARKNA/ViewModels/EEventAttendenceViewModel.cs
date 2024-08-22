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
        public Guid EventstId { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم الحدث")]
        public string EventName { get; set; }  //هنا عشان يعرض اسم الفعالية



        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم الأعضاء بالفعالية")]
        public Guid EventMemId { get; set; }

        //public string EventMemName { get; set; }  //هنا عشان يعرض اسم الأعضاء بالفعالية 


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم الأعضاء بالفعالية")]
        public Guid EventsRegId { get; set; }

        public string Email { get; set; }
        public DateTime RegDate { get; set; }
        public string RegDateString => RegDate.ToString("yyyy-MM-dd HH:mm:ss");

        public string FullNameAr { get; set; }





        public bool IsAttend { get; set; }

    }
}
