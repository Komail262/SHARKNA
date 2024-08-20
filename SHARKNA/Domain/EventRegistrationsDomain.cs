using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class EventRegistrationsDomain
    {
        

        private readonly SHARKNAContext _context;
        public EventRegistrationsDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<EventRegistrationsViewModel> GettblEventRegistrations()
        {
            var EventReg = _context.tblEventRegistrations.Select(x => new EventRegistrationsViewModel
            {
                Id = x.Id,
                RegDate = x.RegDate,
                RejectionReasons = x.RejectionReasons,
                UserName = x.UserName,
                Email = x.Email,
                MobileNumber = x.MobileNumber,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                EventId = x.EventsId,
                RequestStatusId = x.EventStatusId
            }).ToList();
            return EventReg;
        }

        public EventRegistrationsViewModel GettblEventRegistrations(Guid id)
        {

            var EventReg = _context.tblEventRegistrations.FirstOrDefault(u => u.Id == id);
            EventRegistrationsViewModel uu = new EventRegistrationsViewModel();
            uu.Id = id;
            uu.RegDate = EventReg.RegDate;
            uu.RejectionReasons = EventReg.RejectionReasons;
            uu.UserName = EventReg.UserName;
            uu.Email = EventReg.Email;
            uu.MobileNumber = EventReg.MobileNumber;
            uu.FullNameAr = EventReg.FullNameAr;
            uu.FullNameEn = EventReg.FullNameEn;
            return uu;

        }

        public void AddEventReg(EventRegistrationsViewModel EventReg)
        {
            
            


                tblEventRegistrations VEventReg = new tblEventRegistrations();
                VEventReg.Id = EventReg.Id;
                VEventReg.RegDate = EventReg.RegDate;
                VEventReg.RejectionReasons = EventReg.RejectionReasons;
                VEventReg.UserName = EventReg.UserName;
                VEventReg.Email = EventReg.Email;
                VEventReg.MobileNumber = EventReg.MobileNumber;
                VEventReg.FullNameAr = EventReg.FullNameAr;
                VEventReg.FullNameEn = EventReg.FullNameEn;
                VEventReg.EventStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
                VEventReg.EventsId = EventReg.EventId;
                _context.Add(VEventReg);
                _context.SaveChanges();
                

            
        
        }

        
        public bool IsEmailDuplicate(string email, Guid? EventRegId = null)
        {
            if (EventRegId == null)
            {
                return _context.tblEventRegistrations.Any(u => u.Email == email);

            }
            else
            {
                return _context.tblEventRegistrations.Any(u => u.Email == email && u.Id != EventRegId);
            }
        }


        public List<tblEvents> GettblEvents()
        {
            return _context.tblEvents.Where(e => !e.IsDeleted && e.IsActive).ToList();
        }


    }
}
