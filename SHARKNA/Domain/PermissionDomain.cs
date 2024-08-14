using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.EntityFrameworkCore;
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
            return _context.tblPermissions.Include(R => R.Role).Where(I => I.IsDeleted == false).Select(x => new PermissionsViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                IsDeleted = x.IsDeleted,
                RoleId = x.RoleId,
                RoleName = x.Role.NameAr,
            }).ToList();
        }

        public async Task<PermissionsViewModel> GetTblPermissionsById(Guid id)
        {
            var TPermission = await _context.tblPermissions.FirstOrDefaultAsync(g => g.Id == id);
            PermissionsViewModel gg = new PermissionsViewModel();
            gg.Id = id;
            gg.UserName = TPermission.UserName;
            gg.FullNameAr = TPermission.FullNameAr;
            gg.FullNameEn = TPermission.FullNameEn;
            return gg;

        }

        public async Task<UserViewModel> GetTblUsersByUserName(string userName)
        {
            var user = await _context.tblUsers.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }

            return new UserViewModel
            {
                UserName = user.UserName,
                FullNameAr = user.FullNameAr,
                FullNameEn = user.FullNameEn
            };
        }


        public int AddPermission(PermissionsViewModel Permission)
        {
            try
            {
                tblPermissions permission = new tblPermissions();

                permission.Id = Permission.Id;
                permission.UserName = Permission.UserName;
                permission.FullNameAr = Permission.FullNameAr;
                permission.FullNameEn = Permission.FullNameEn;
                permission.RoleId = Permission.RoleId;
                permission.IsDeleted = false;

                _context.tblPermissions.Add(permission);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }
       
        public int UpdatePermission(PermissionsViewModel Permission)
        {
            try
            {
                var existingPermission = _context.tblPermissions.FirstOrDefault(e => e.Id == Permission.Id);
                if (existingPermission == null)
                {
                    return 0;
                }

                existingPermission.UserName = Permission.UserName;
                existingPermission.FullNameAr = Permission.FullNameAr;
                existingPermission.FullNameEn = Permission.FullNameEn;
                existingPermission.RoleId = Permission.RoleId;
                existingPermission.IsDeleted = false;
                _context.Update(existingPermission);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public int DeletePermission(Guid id)
        {
            try
            {
                var Per = _context.tblPermissions.FirstOrDefault(b => b.Id == id);
                if (Per != null)
                {
                    Per.IsDeleted = true;
                    _context.Update(Per);
                    _context.SaveChanges();

                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public bool IsRoleNameDuplicate(string name, Guid? Permissionn = null)
        {
            if (Permissionn == null)
            {
                return _context.tblPermissions.Any(u => u.UserName == name);

            }
            else
            {
                return _context.tblPermissions.Any(u => u.UserName == name && u.Id != Permissionn);
            }
        }

    }
}