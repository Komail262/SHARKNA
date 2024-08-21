using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.ViewModels
{
    public class BoardRequestsViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [DisplayName("اسم النادي")]
        public Guid BoardId { get; set; }

        public string BoardName { get; set; }

        
        [DisplayName("حالة الطلب")]
        public Guid RequestStatusId { get; set; }

        public string RequestStatusName { get; set; }

        public ICollection<tblBoards> BoardsOfList { get; set; }
        //public ICollection<tblRequestStatus> ReqStatusOfList { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(15)]
        [DisplayName("رقم الجوال")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم بالعربي")]
        public string FullNameAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(100)]
        [DisplayName("الاسم بالانجليزي")]
        public string FullNameEn { get; set; }

        [StringLength(100)]
        [DisplayName(" سبب الرفض")]
        public string? RejectionReasons { get; set; }

        //new

    }
}
