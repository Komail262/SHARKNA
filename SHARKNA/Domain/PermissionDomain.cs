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

        public void AddPermission(PermissionsViewModel Permission)
        {
            tblPermissions Permissions = new tblPermissions();
            Permissions.Id = Permission.Id;
            Permissions.UserName = Permission.UserName;
            Permissions.FullNameAr = Permission.FullNameAr;
            Permissions.FullNameEn = Permission.FullNameEn;
            Permissions.RoleId = Guid.Parse("27BDCFC2-532F-4B34-AC91-AE5E0A84731F");
            Permissions.IsDeleted = false;

            _context.tblPermissions.Add(Permissions);
            _context.SaveChanges();
        }
        public int DeleteBoard(Guid id)
        {
            try
            {
                var board = _context.tblBoards.FirstOrDefault(b => b.Id == id);
                if (board != null)
                {
                    board.IsDeleted = true;
                    board.IsActive = false;
                    _context.Update(board);
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
                _context.Update(existingPermission);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }


    }
}