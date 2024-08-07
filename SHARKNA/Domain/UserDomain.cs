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

        //public RoleDomain GetTblRoleById(Guid id)
        //{
        //    var role = _context.tblRole.FirstOrDefault(u => u.Id == id);
        //    RolesViewModel rr = new RolesViewModel();
        //    rr.Id = id;
        //    rr.NameAr = role.NameAr;
        //    rr.NameEn = role.NameEn;
        //    return rr;
        //}

        // Function to get the Guid ID using email
        //public int? GetUserIdByEmail(string email)
        //{
        //    // Retrieve the user ID based on the email
        //    var userId = _context.tblUsers
        //        .Where(u => u.Email == email)
        //        .Select(u => u.Id)
        //        .FirstOrDefault();

        //    // If user is not found, return null
        //    return userId == 0 ? (int?)null : userId;
        //}

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
                // Check if the username already exists for new users
                return _context.tblUsers.Any(u => u.UserName == username);
            }
            else
            {
                // Check if the username is duplicated for existing users but exclude the current user from the check
                return _context.tblUsers.Any(u => u.UserName == username && u.Id != userId);
            }
        }


        public int Login(UserLoginViewModel user)
        {
            try
            {
                // Check if any user matches either the email or username with the provided password
                if (_context.tblUsers.Any(u => (u.Email == user.Email || u.UserName == user.UserName) && u.Password == user.Password))
                {
                    return 1; // Login successful
                }
                else
                {
                    return 0; // Invalid email/username or password
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here using your logging framework
                // Example: _logger.LogError(ex, "An error occurred during login");
            }
            return 0; // Error occurred
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





    }

}
