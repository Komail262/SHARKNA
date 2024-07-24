using System.Collections.Generic;
using System.Linq;
using SHARKNA.Models;

namespace SHARKNA.Domain
{
    public class UserDomain
    {
        private readonly SHARKNAContext _context;
        public UserDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<tblUsers> GetTblUsers()
        {
            return _context.tblUsers.ToList();
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
    }
}
