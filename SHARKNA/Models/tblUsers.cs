using System;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.Models
{
    public class tblUsers
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();


        
        public string UserName { get; set; }

        
        public string Password { get; set; }

        
        public string Email { get; set; }

        
        public string MobileNumber { get; set; }

        
        public string FullNameAr { get; set; }

        
        public string FullNameEn { get; set; }

        public bool Gender { get; set; }

        public string UserType { get; set; }
    }
}
