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

        public async Task AddEventRegAsync(EventRegistrationsViewModel EventReg)
        {
            // Create a new instance of tblEventRegistrations
            tblEventRegistrations VEventReg = new tblEventRegistrations
            {
                Id = EventReg.Id,
                RegDate = EventReg.RegDate,
                RejectionReasons = EventReg.RejectionReasons,
                UserName = EventReg.UserName,
                Email = EventReg.Email,
                MobileNumber = EventReg.MobileNumber,
                FullNameAr = EventReg.FullNameAr,
                FullNameEn = EventReg.FullNameEn,
                EventStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2"),
                EventsId = EventReg.EventId
            };

           
            await _context.AddAsync(VEventReg);

           
            await _context.SaveChangesAsync();
        }





        public IEnumerable<EventRegistrationsViewModel> GetUserRegisteredEvents(string username)
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
