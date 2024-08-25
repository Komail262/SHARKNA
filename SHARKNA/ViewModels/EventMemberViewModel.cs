using SHARKNA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SHARKNA.ViewModels
{
    public class EventMemberViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم كاملا بالعربي")]
        public string FullNameAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم كاملا بالانجليزي")]
        public string FullNameEn { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("رقم الجوال")]
        public string MobileNumber { get; set; }


        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        //public ICollection<tblEventAttendence> EventAttend { get; set; }
    }
}
