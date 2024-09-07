using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class UserDomain
    {
        private readonly SHARKNAContext _context;
        public UserDomain(SHARKNAContext context)
        {
            _context = context;
        }



        public IEnumerable<UserViewModel> GetTblUsers()
        {
            return _context.tblUsers.Select(x => new UserViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                MobileNumber = x.MobileNumber
            }).ToList();
        }

        public UserViewModel GetTblUserById(Guid id)
        {
            var user = _context.tblUsers.FirstOrDefault(u => u.Id == id);
            UserViewModel uu = new UserViewModel();
            uu.Id = id;
            uu.UserName = user.UserName;
            uu.Password = user.Password;
            uu.Email = user.Email;
            uu.FullNameAr = user.FullNameAr;
            uu.MobileNumber = user.MobileNumber;
            uu.FullNameEn = user.FullNameEn;
            return uu;



        }


        public void AddUser(UserViewModel user)
        {
            tblUsers Vuser = new tblUsers();
            Vuser.Id = user.Id;
            Vuser.UserName = user.UserName;
            Vuser.Password = user.Password;
            Vuser.Email = user.Email;
            Vuser.FullNameAr = user.FullNameAr;
            Vuser.FullNameEn = user.FullNameEn;
            Vuser.MobileNumber = user.MobileNumber;

            _context.tblUsers.Add(Vuser);
            _context.SaveChanges();
        }

        public void UpdateUser(UserViewModel user)

        {
            tblUsers Vuser = new tblUsers();
            Vuser.Id = user.Id;
            Vuser.UserName = user.UserName;
            Vuser.Password = user.Password;
            Vuser.Email = user.Email;
            Vuser.FullNameAr = user.FullNameAr;
            Vuser.FullNameEn = user.FullNameEn;
            Vuser.MobileNumber = user.MobileNumber;

            _context.tblUsers.Update(Vuser);
            _context.SaveChanges();
        }

        public bool IsEmailDuplicate(String email, Guid? userId = null)
        {
            if (userId == null)
            {
                return _context.tblUsers.Any(u => u.Email == email);

            }
            else
            {
                return _context.tblUsers.Any(u => u.Email == email && u.Id != userId);
            }
        }

        public bool IsUserNameDuplicate(string username, Guid? userId = null)
        {
            if (userId == null)
            {
                
                return _context.tblUsers.Any(u => u.UserName == username);
            }
            else
            {
                
                return _context.tblUsers.Any(u => u.UserName == username && u.Id != userId);
            }
        }

        public async Task<UserViewModel> GetTblUserByUserName(string UserName)
        {
            var x = await _context.tblUsers.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (x != null)
            {
                return new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Password = x.Password,
                    Email = x.Email,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    MobileNumber = x.MobileNumber
                };
            }
            else
                return null;
        }

        public PermissionsViewModel GetUserByUsername(string username)
{
    var userWithRole = (from u in _context.tblPermissions
                        join r in _context.tblRoles on u.RoleId equals r.Id
                        where u.UserName == username
                        select new { u, r }).FirstOrDefault();

    if (userWithRole == null)
    {
        return null;
    }

    return new PermissionsViewModel
    {
        Id = userWithRole.u.Id,
        UserName = userWithRole.u.UserName,
        FullNameAr = userWithRole.u.FullNameAr,
        FullNameEn = userWithRole.u.FullNameEn,
        RoleId = userWithRole.u.RoleId,
        RoleName = userWithRole.r.NameEn
    };
}

        public UserViewModel GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _context.tblUsers.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user == null)
            {
                return null;
            }

            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullNameAr = user.FullNameAr,
                FullNameEn = user.FullNameEn,
                Email = user.Email,
                MobileNumber = user.MobileNumber
            };
        }





    }

}
