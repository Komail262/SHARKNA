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

        public string EventName { get; set; }  //هنا عشان يعرض اسم الفعالية



        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم الأعضاء بالفعالية")]
        public Guid EventMemId { get; set; }

        public string EventMemName { get; set; }  //هنا عشان يعرض اسم الأعضاء بالفعالية 




        public bool IsAttend { get; set; }

    }
}
