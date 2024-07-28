using System;
using System.ComponentModel.DataAnnotations;

namespace SHARKNA.Models //ziyad
{
    public class tblPermissions
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public bool IsDeleted { get; set; }
        public tblRoles Role { get; set; }
        public Guid RoleId { get; set; }
    }
}
