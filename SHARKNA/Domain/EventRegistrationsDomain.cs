using System;
using System.Collections.Generic;
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

        public void AddEventReg(EventRegistrationsViewModel eventReg)
        {
            var tblEventReg = new tblEventRegistrations
            {
                Id = eventReg.Id,
                RegDate = eventReg.RegDate,
                RejectionReasons = eventReg.RejectionReasons,
                UserName = eventReg.UserName,
                Email = eventReg.Email,
                MobileNumber = eventReg.MobileNumber,
                FullNameAr = eventReg.FullNameAr,
                FullNameEn = eventReg.FullNameEn,
                EventStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2"), 
                EventsId = eventReg.EventId
            };
            _context.tblEventRegistrations.Add(tblEventReg);
            _context.SaveChanges();
        }

        public List<EventViewModel> GetEventsForUser(string username)
        {
            var events = _context.tblEvents
                .Where(e => !e.IsDeleted && e.IsActive)
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    EventTitleAr = e.EventTitleAr,
                    EventStartDate = e.EventStartDate,
                    EventEndtDate = e.EventEndtDate,
                    LocationAr = e.LocationAr,
                    SpeakersAr = e.SpeakersAr,
                    TopicAr = e.TopicAr,
                    DescriptionAr = e.DescriptionAr,
                 
                })
                .ToList();

            return events;
        }


        public EventViewModel GetEventById(Guid eventId)
        {
            return _context.tblEvents
                .Where(e => e.Id == eventId)
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    EventTitleAr = e.EventTitleAr,
                    EventStartDate = e.EventStartDate,
                    EventEndtDate = e.EventEndtDate,
                    LocationAr = e.LocationAr,
                    SpeakersAr = e.SpeakersAr,
                    TopicAr = e.TopicAr,
                    DescriptionAr = e.DescriptionAr
                })
                .FirstOrDefault();
        }

        public int GetEventRegistrationsCount(Guid eventId)
        {
            return _context.tblEventRegistrations
                           .Count(reg => reg.EventsId == eventId);
        }


        




    }
}
