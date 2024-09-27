using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace SHARKNA.ViewModels
{
    public class BoardViewModel
    {

        public Guid Id { get; set; }
        //first

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم اللجنة ")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم اللجنة بالإنجليزي")]
        public string NameEn { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(5000)]
        [DisplayName("الوصف بالعربي")]
        public string DescriptionAr { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(5000)]
        [DisplayName("الوصف بالإنجليزي")]
        public string DescriptionEn { get; set; }

        [DisplayName("الجنس")]
        public bool? Gender { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        
        [DisplayName("محذوف؟")]
        public bool IsDeleted { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        
        [DisplayName("نشط؟")]
        public bool IsActive { get; set; }



    }
}
