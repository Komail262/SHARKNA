using System.Collections.Generic;
using System.Linq;
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
            var Tuser = _context.tblUsers.FirstOrDefault(u => u.Id == id);
            UserViewModel uu = new UserViewModel();
            uu.Id = id;
            uu.UserName = Tuser.UserName;
            uu.Password = Tuser.Password;
            uu.Email = Tuser.Email;
            uu.FullNameAr = Tuser.FullNameAr;
            uu.MobileNumber = Tuser.MobileNumber;
            uu.FullNameEn = Tuser.FullNameEn;
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

        public bool IsEmailDuplicate(string email, Guid? userId = null)
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
    }
}
