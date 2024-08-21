using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        
       

        public IEnumerable<EventRegistrationsViewModel> GetUserRegisteredEvents(string username)
        {
            return _context.tblEventRegistrations
                .Where(reg => reg.UserName == username)
                .Select(reg => new EventRegistrationsViewModel
                {
                    Id = reg.Id,
                    RegDate = reg.RegDate,
                    RejectionReasons = reg.RejectionReasons,
                    UserName = reg.UserName,
                    Email = reg.Email,
                    MobileNumber = reg.MobileNumber,
                    FullNameAr = reg.FullNameAr,
                    FullNameEn = reg.FullNameEn,
                    EventId = reg.EventsId

                })
                .ToList();
        }


        public List<EventViewModel> GetEventsForUser(string username)
        {

            var events = _context.tblEvents.Where(e => !e.IsDeleted && e.IsActive).ToList();

            var registeredEvents = _context.tblEventRegistrations
                .Where(reg => reg.UserName == username)
                .Select(reg => reg.EventsId)
                .ToList();


            var availableEvents = events
                .Where(e => !registeredEvents.Contains(e.Id))
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    EventTitleAr = e.EventTitleAr,
                    EventStartDate = e.EventStartDate,
                    EventEndtDate = e.EventEndtDate,
                    Time = e.Time,
                    LocationAr = e.LocationAr,
                    SpeakersAr = e.SpeakersAr,
                    TopicAr = e.TopicAr,
                    DescriptionAr = e.DescriptionAr
                }).ToList();

            return availableEvents;
        }

        




    }

}

