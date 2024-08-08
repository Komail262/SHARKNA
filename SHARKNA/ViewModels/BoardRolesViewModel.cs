using SHARKNA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;




namespace SHARKNA.ViewModels
{
    public class BoardRolesViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم العربي")]
        
        public string NameAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم بالانجليزي")]
        public string NameEn { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }





    }
}


