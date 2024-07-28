using System.Collections.Generic;
using System.Linq;
using System.Security;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class PermissionDomain
    {
        private readonly SHARKNAContext _context;
        public PermissionDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<PermissionsViewModel> GetTblPermissions()
        {
            return _context.tblPermissions.Select(x => new PermissionsViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                IsDeleted = x.IsDeleted,

            }).ToList();
        }

        public PermissionsViewModel GetTblPermissionsById(Guid id)
        {
            var TPermission = _context.tblPermissions.FirstOrDefault(g => g.Id == id);
            PermissionsViewModel gg = new PermissionsViewModel();
            gg.Id = id;
            gg.UserName = TPermission.UserName;
            gg.FullNameAr = TPermission.FullNameAr;
            gg.FullNameEn = TPermission.FullNameEn;
            return gg;

        }

        public void AddPermission(PermissionsViewModel Permission)
        {
            tblPermissions Permissions = new tblPermissions();
            Permissions.Id = Permissions.Id;
            Permissions.UserName = Permissions.UserName;           
            Permissions.FullNameAr = Permissions.FullNameAr;
            Permissions.FullNameEn = Permissions.FullNameEn;

            _context.tblPermissions.Add(Permissions);
            _context.SaveChanges();
        }

        public void UpdatePermission(PermissionsViewModel Permission)

        {
            tblPermissions VPermission = new tblPermissions();
            VPermission.Id = VPermission.Id;
            VPermission.UserName = VPermission.UserName;
            VPermission.FullNameAr = VPermission.FullNameAr;
            VPermission.FullNameEn = VPermission.FullNameEn;

            _context.tblPermissions.Update(VPermission);
            _context.SaveChanges();
        }

        //public bool IsEmailDuplicate(string email, Guid? userId = null)
        //{
        //    if (userId == null)
        //    {
        //        return _context.tblUsers.Any(u => u.Email == email);

        //    }
        //    else
        //    {
        //        return _context.tblUsers.Any(u => u.Email == email && u.Id != PermissionId);
        //    }
        //}
    }
}