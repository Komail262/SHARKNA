using System.Collections.Generic;
using System.Linq;
using System.Security;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class RoleDomain
    {
        private readonly SHARKNAContext _context;
        public RoleDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<RolesViewModel> GetTblRoles()
        {
            return _context.tblRoles.Select(r => new RolesViewModel
            {
                Id = r.Id,
                NameAr = r.NameAr,
                NameEn = r.NameEn,

            }).ToList();
        }

        public RolesViewModel GetTblRolesById(Guid id)
        {
            var TRole = _context.tblRoles.FirstOrDefault(g => g.Id == id);
            RolesViewModel gg = new RolesViewModel();
            gg.Id = id;
            gg.NameAr = TRole.NameAr;
            gg.NameEn = TRole.NameEn;
            return gg;

        }

        public void AddRole(RolesViewModel Role)
        {
            tblRoles Roles = new tblRoles();
            Role.NameAr = Role.NameAr;
            Role.NameEn = Role.NameEn;

            _context.tblRoles.Add(Roles);
            _context.SaveChanges();
        }

        public void UpdateRoles(RolesViewModel Role)

        {
            tblRoles VRole = new tblRoles();
            VRole.Id = VRole.Id;
            VRole.NameAr = VRole.NameAr;
            VRole.NameEn = VRole.NameEn;

            _context.tblRoles.Update(VRole);
            _context.SaveChanges();
        }

        public List<tblRoles> GettblRoles()
        {
            return _context.tblRoles.ToList();
        }
    }
}