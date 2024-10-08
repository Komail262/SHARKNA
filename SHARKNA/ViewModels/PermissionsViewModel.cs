﻿using SHARKNA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;




namespace SHARKNA.ViewModels
{
    public class PermissionsViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم بالعربي")]
        public string FullNameAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم بالانجليزي")]
        public string FullNameEn { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName(" قائمة الادوار")]
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        //public ICollection<tblRoles> RolesId { get; set; }


    }
}


