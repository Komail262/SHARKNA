using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.ViewModels
{
    public class UserLoginViewModel
    {
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
        [DisplayName("كلمة المرور")]
        public string Password { get; set; }

     
    }
}
