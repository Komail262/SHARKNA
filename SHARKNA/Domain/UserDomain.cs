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
            return _context.tblUsers.Select( x => new UserViewModel
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

        public tblUsers GetTblUserById(Guid id)
        {
            return _context.tblUsers.FirstOrDefault(u => u.Id == id);
        }

        public void AddUser(tblUsers user)
        {
            _context.tblUsers.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(tblUsers user)
        {
            _context.tblUsers.Update(user);
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
    }
}
