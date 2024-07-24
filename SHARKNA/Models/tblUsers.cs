using System;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.Models
{
    public class tblUsers
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FullNameAr { get; set; }

        [Required]
        [StringLength(100)]
        public string FullNameEn { get; set; }
    }
}
