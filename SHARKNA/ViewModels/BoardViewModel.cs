using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SHARKNA.ViewModels
{
    public class BoardViewModel
    {

        public Guid Id { get; set; }
        //first

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم اللجنة بالعربي")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم اللجنة بالإنجليزي")]
        public string NameEn { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الوصف بالعربي")]
        public string DescriptionAr { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الوصف بالإنجليزي")]
        public string DescriptionEn { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        
        [DisplayName("محذوف؟")]
        public bool IsDeleted { get; set; }


        [Required(ErrorMessage = "هذا الحقل اجباري")]
        
        [DisplayName("نشط؟")]
        public bool IsActive { get; set; }
    }
}
