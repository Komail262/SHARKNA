
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class EventMembersDomain
    {
        private readonly SHARKNAContext _context;
        public EventMembersDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<EventMemberViewModel> GetTblEventMembers()
        {
            return _context.tblEventMembers.Where(i => i.IsDeleted == false).Select(x => new EventMemberViewModel
            {
                Id = x.Id,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                MobileNumber = x.MobileNumber,
                IsDeleted = x.IsDeleted,
                IsActive = x.IsActive
            }).ToList();
        }

        public EventMemberViewModel GetTblEventMemberById(Guid id)
        {
            var EMem = _context.tblEventMembers.FirstOrDefault(b => b.Id == id);
            if (EMem == null) return null;
            EventMemberViewModel EM = new EventMemberViewModel();
            EM.Id = EMem.Id;
            EM.FullNameAr = EMem.FullNameAr;
            EM.FullNameEn = EMem.FullNameEn;
            EM.MobileNumber = EMem.MobileNumber;
            EM.IsDeleted = EMem.IsDeleted;
            EM.IsActive = EMem.IsActive;
            return EM;
           
        }

        //public int AddEventMember(EventMemberViewModel member)
        //{
        //    try
        //    {
        //        tblEventMembers AEv = new tblEventMembers();

        //        AEv.Id = member.Id;
        //        AEv.FullNameAr = member.FullNameAr;
        //        AEv.FullNameEn = member.FullNameEn;
        //        AEv.MobileNumber = member.MobileNumber;
        //        AEv.IsDeleted = false;
        //        AEv.IsActive = true;

        //        _context.tblBoards.Add(AEv);
        //        _context.SaveChanges();
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;

        //    }
        //}
    }
}
