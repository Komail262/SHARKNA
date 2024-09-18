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


        public async Task<int> AddPermissionAsync(PermissionsViewModel Permission, string username)
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

                tblPermmisionLogs Plogs = new tblPermmisionLogs();
                Plogs.Id = Guid.NewGuid();
                Plogs.Id = permission.Id;
                Plogs.OpType = "إضافة";
                Plogs.DateTime = DateTime.Now;
                Plogs.CreatedBy = username;
                Plogs.ModifiedBy = username;
                Plogs.CreatedTo = permission.FullNameAr;
                Plogs.AdditionalInfo = $"تم إضافة صلاحية {permission.FullNameAr} بواسطة هذا المستخدم {username}";
                _context.tblPermmisionLogs.Add(Plogs);

                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }

        public async Task<int> UpdatePermissionAsync(PermissionsViewModel Permission, string username)
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

                tblPermmisionLogs Plogs = new tblPermmisionLogs();
                Plogs.Id = Guid.NewGuid();
                Plogs.Id = existingPermission.Id;
                Plogs.OpType = "تعديل";
                Plogs.DateTime = DateTime.Now;
                Plogs.CreatedBy = username;
                Plogs.ModifiedBy = username;
                Plogs.CreatedTo = existingPermission.FullNameAr;
                Plogs.AdditionalInfo = $"تم تعديل صلاحية {existingPermission.FullNameAr} بواسطة هذا المستخدم {username}";
                _context.tblPermmisionLogs.Add(Plogs);

                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public async Task<int> DeletePermissionAsync(Guid id, string username)
        {
            try
            {
                PermissionsViewModel Permission = new PermissionsViewModel();
                var Per = _context.tblPermissions.FirstOrDefault(b => b.Id == id);
                if (Per != null)
                {
                    Per.IsDeleted = true;
                    _context.Update(Per);
                    _context.SaveChanges();

                    tblPermmisionLogs Plogs = new tblPermmisionLogs();
                    Plogs.Id = Guid.NewGuid();
                    Plogs.Id = Per.Id;
                    Plogs.OpType = "حذف";
                    Plogs.DateTime = DateTime.Now;
                    Plogs.CreatedBy = username;
                    Plogs.ModifiedBy = username;
                    Plogs.CreatedTo = Per.FullNameAr;
                    Plogs.AdditionalInfo = $"تم حذف  صلاحية {Per.FullNameAr} بواسطة هذا المستخدم {username}";
                    _context.tblPermmisionLogs.Add(Plogs);

                    await _context.SaveChangesAsync();

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
                // Check if there is any non-deleted permission with the given user name
                return _context.tblPermissions.Any(u => u.UserName == name && !u.IsDeleted);
            }
            else
            {
                // Check if there is any non-deleted permission with the given user name
                // excluding the permission with the specified Permissionn ID
                return _context.tblPermissions.Any(u => u.UserName == name && u.Id != Permissionn && !u.IsDeleted);
            }
        }

    }

}