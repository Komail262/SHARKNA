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

        
        public IEnumerable<tblUsers> GettblUsers()
        {
            return _context.tblUsers.ToList();
        }
    }
}
