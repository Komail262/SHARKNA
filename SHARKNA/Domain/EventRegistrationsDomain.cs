using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class EventRegistrationsDomain
    {
        private readonly SHARKNAContext _context;
        private readonly EventDomain _eventDomain;


        public EventRegistrationsDomain(SHARKNAContext context, EventDomain eventDomain)
        {
            _context = context;
            _eventDomain = eventDomain;
        }

        public async Task<IEnumerable<EventRegistrationsViewModel>> GetUserRegisteredEventsAsync(string username)
        {
            return await _context.tblEventRegistrations
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
                .ToListAsync();
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


        public async Task AddEventRegAsync(EventRegistrationsViewModel eventReg)
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
            await _context.SaveChangesAsync();


            var log = new tblEventRegLogs
            {
                Id = Guid.NewGuid(),
                OpType = "Registration",
                OpDateTime = DateTime.Now,
                CreatedBy = eventReg.FullNameAr,
                ModifiedBy = eventReg.FullNameAr,
                AdditionalInfo = $"Event registered by user {eventReg.UserName}",
                EvId = eventReg.EventId
            };

            _context.tblEventRegLogs.Add(log);
            await _context.SaveChangesAsync();



            var ev = _eventDomain.GetEventById(eventReg.EventId);
            var days = (ev.EventEndtDate.Value - ev.EventStartDate.Value).Days + 1;

            for (int i = 0; i < days; i++)
            {
                tblEventAttendence atten = new tblEventAttendence();
                atten.Id = Guid.NewGuid();
                atten.EventsId = eventReg.EventId;
                atten.EventsRegId = eventReg.Id;
                atten.Day = i + 1;
                atten.IsAttend = false;
                atten.EventDate = ev.EventStartDate.Value.AddDays(i);
                _context.tblEventAttendence.Add(atten);
                _context.SaveChanges();

            }

        }

        public async Task<List<EventViewModel>> GetEventsForUserAsync(string username)
        {
            return await _context.tblEvents
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
                    DescriptionAr = e.DescriptionAr
                })
                .ToListAsync();
        }

        public async Task<EventViewModel> GetEventByIdAsync(Guid eventId)
        {
            return await _context.tblEvents
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
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetEventRegistrationsCountAsync(Guid eventId)
        {
            return await _context.tblEventRegistrations
                .CountAsync(reg => reg.EventsId == eventId);
        }
    }
}
