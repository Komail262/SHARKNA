using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace SHARKNA.ViewModels
{
    public class BoardMembersViewModel
    {
        public Guid Id { get; set; }

        
        [StringLength(100)]
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; }

        
        [StringLength(100)]
        [DisplayName("البريد الإلكتروني")]
        public string Email { get; set; }

        
        [StringLength(15)]
        [DisplayName("رقم الجوال")]
        public string MobileNumber { get; set; }

        
        [StringLength(100)]
        [DisplayName("الاسم بالعربي")]
        public string FullNameAr { get; set; }


       
        [StringLength(100)]
        [DisplayName("الاسم بالانجليزي")]
        public string FullNameEn { get; set; }


        
        [DisplayName("اسم النادي")]
        public Guid BoardId { get; set; }
        public string BoardName { get; set; }

        [DisplayName("نوع العضوية")]
        public Guid BoardRoleId { get; set; }
        [DisplayName("نوع العضوية")]
        public string BoardRoleName {  get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

       
    }
}
