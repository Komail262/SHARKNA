using System.Collections.Generic;
using System.Linq;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class EventDomain
    {
        private readonly SHARKNAContext _context;
        public EventDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<EventViewModel> GettblEvents()
        {
            return _context.tblEvents.Where(i => i.IsDeleted == false).Select(E => new EventViewModel
            {

                Id = E.Id,
                EventTitleAr = E.EventTitleAr,
                EventTitleEn = E.EventTitleEn,
                EventStartDate = E.EventStartDate,
                EventEndtDate = E.EventEndtDate,
                SpeakersAr = E.SpeakersAr,
                SpeakersEn = E.SpeakersEn,
                TopicAr = E.TopicAr,
                TopicEn = E.TopicEn,
                DescriptionAr = E.DescriptionAr,
                DescriptionEn = E.DescriptionEn,
                LocationAr = E.LocationAr,
                LocationEn = E.LocationEn,
                MaxAttendence = E.MaxAttendence,
                IsActive = E.IsActive,
                IsDeleted = E.IsDeleted,


            }).ToList();
        }

        public EventViewModel GetTblEventsById(Guid id)
        {
            var Tuser = _context.tblEvents.FirstOrDefault(u => u.Id == id);
            EventViewModel uu = new EventViewModel();
            uu.Id = id;
            uu.EventTitleAr = Tuser.EventTitleAr;
            uu.EventTitleEn = Tuser.EventTitleEn;
            uu.EventStartDate = Tuser.EventStartDate;
            uu.EventEndtDate = Tuser.EventEndtDate;
            uu.SpeakersAr = Tuser.SpeakersAr;
            uu.SpeakersEn = Tuser.SpeakersEn;
            uu.TopicAr = Tuser.TopicAr;
            uu.TopicEn = Tuser.TopicEn;
            uu.DescriptionAr = Tuser.DescriptionAr;
            uu.DescriptionEn = Tuser.DescriptionEn;
            uu.LocationAr = Tuser.LocationAr;
            uu.LocationEn = Tuser.LocationEn;
            uu.LocationEn = Tuser.LocationEn;
            uu.MaxAttendence = Tuser.MaxAttendence;
            uu.IsActive = Tuser.IsActive;
            uu.IsDeleted = Tuser.IsDeleted;
            return uu;

        }

        public int AddEvent(EventViewModel Event)
        {
            try
            {
                tblEvents VEvent = new tblEvents();

                VEvent.Id = Event.Id;
                VEvent.EventTitleAr = Event.EventTitleAr;
                VEvent.EventTitleEn = Event.EventTitleEn;
                VEvent.EventStartDate = Event.EventStartDate;
                VEvent.EventEndtDate = Event.EventEndtDate;
                VEvent.SpeakersAr = Event.SpeakersAr;
                VEvent.SpeakersEn = Event.SpeakersEn;
                VEvent.TopicAr = Event.TopicAr;
                VEvent.TopicEn = Event.TopicEn;
                VEvent.DescriptionAr = Event.DescriptionAr;
                VEvent.DescriptionEn = Event.DescriptionEn;
                VEvent.LocationAr = Event.LocationAr;
                VEvent.LocationEn = Event.LocationEn;
                VEvent.MaxAttendence = Event.MaxAttendence;
                VEvent.IsActive = Event.IsActive;
                VEvent.IsDeleted = Event.IsDeleted;

                _context.tblEvents.Add(VEvent);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }


            
        }

        public int UpdateEvent(EventViewModel Event)
        {
            try
            {
                var existingEvent = _context.tblEvents.FirstOrDefault(e => e.Id == Event.Id);
                if (existingEvent == null)
                {
                    return 0;
                }

                existingEvent.EventTitleAr = Event.EventTitleAr;
                existingEvent.EventTitleEn = Event.EventTitleEn;
                existingEvent.EventStartDate = Event.EventStartDate;
                existingEvent.EventEndtDate = Event.EventEndtDate;
                existingEvent.SpeakersAr = Event.SpeakersAr;
                existingEvent.SpeakersEn = Event.SpeakersEn;
                existingEvent.TopicAr = Event.TopicAr;
                existingEvent.TopicEn = Event.TopicEn;
                existingEvent.DescriptionAr = Event.DescriptionAr;
                existingEvent.DescriptionEn = Event.DescriptionEn;
                existingEvent.LocationAr = Event.LocationAr;
                existingEvent.LocationEn = Event.LocationEn;
                existingEvent.MaxAttendence = Event.MaxAttendence;
                existingEvent.IsDeleted = false;
                existingEvent.IsActive = true;

                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                 
                return 0;
            }
        }

        public int DeleteEvent(Guid id)
        {
            try
            {
                var DEvent = _context.tblEvents.FirstOrDefault(b => b.Id == id);
                if (DEvent != null)
                {
                    DEvent.IsDeleted = true;
                    DEvent.IsActive = false;
                    _context.Update(DEvent);
                    _context.SaveChanges();

                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
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

    }
}
