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
            return _context.tblEvents.Where(i => i.IsActive == true).Select(E => new EventViewModel
            {

                Id = E.Id,
                EventTitleAr = E.EventTitleAr,
                EventTitleEn = E.EventTitleEn,
                EventStartDate = E.EventStartDate,
                EventEndtDate = E.EventEndtDate,
                Time = E.Time,
                EndRegTime = E.EndRegTime,
                SpeakersAr = E.SpeakersAr,
                SpeakersEn = E.SpeakersEn,
                TopicAr = E.TopicAr,
                TopicEn = E.TopicEn,
                DescriptionAr = E.DescriptionAr,
                DescriptionEn = E.DescriptionEn,
                LocationAr = E.LocationAr,
                LocationEn = E.LocationEn,
                IsActive = E.IsActive,
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
            uu.Time = Tuser.Time;
            uu.EndRegTime = Tuser.EndRegTime;
            uu.SpeakersAr = Tuser.SpeakersAr;
            uu.SpeakersEn = Tuser.SpeakersEn;
            uu.TopicAr = Tuser.TopicAr;
            uu.TopicEn = Tuser.TopicEn;
            uu.DescriptionAr = Tuser.DescriptionAr;
            uu.DescriptionEn = Tuser.DescriptionEn;
            uu.LocationAr = Tuser.LocationAr;
            uu.LocationEn = Tuser.LocationEn;
            uu.LocationEn = Tuser.LocationEn;
            uu.IsActive = Tuser.IsActive;
            return uu;

        }

        public void AddEvent(EventViewModel Event)
        {
            tblEvents VEvent = new tblEvents();
            VEvent.Id = Event.Id;
            VEvent.EventTitleAr = Event.EventTitleAr;
            VEvent.EventTitleEn = Event.EventTitleEn;
            VEvent.EventStartDate = Event.EventStartDate;
            VEvent.EventEndtDate = Event.EventEndtDate;
            VEvent.Time = Event.Time;
            VEvent.EndRegTime = Event.EndRegTime;
            VEvent.SpeakersAr = Event.SpeakersAr;
            VEvent.SpeakersEn = Event.SpeakersEn;
            VEvent.TopicAr = Event.TopicAr;
            VEvent.TopicEn = Event.TopicEn;
            VEvent.DescriptionAr = Event.DescriptionAr;
            VEvent.DescriptionEn = Event.DescriptionEn;
            VEvent.LocationAr = Event.LocationAr;
            VEvent.LocationEn = Event.LocationEn;
            VEvent.IsActive = Event.IsActive;
            

            _context.tblEvents.Add(VEvent);
            _context.SaveChanges();
        }

        public void UpdateEvent(EventViewModel Event)

        {
            tblEvents VEvent = new tblEvents();
            
            VEvent.EventTitleAr = Event.EventTitleAr;
            VEvent.EventTitleEn = Event.EventTitleEn;
            VEvent.EventStartDate = Event.EventStartDate;
            VEvent.EventEndtDate = Event.EventEndtDate;
            VEvent.Time = Event.Time;
            VEvent.EndRegTime = Event.EndRegTime;
            VEvent.SpeakersAr = Event.SpeakersAr;
            VEvent.SpeakersEn = Event.SpeakersEn;
            VEvent.TopicAr = Event.TopicAr;
            VEvent.TopicEn = Event.TopicEn;
            VEvent.DescriptionAr = Event.DescriptionAr;
            VEvent.DescriptionEn = Event.DescriptionEn;
            VEvent.LocationAr = Event.LocationAr;
            VEvent.LocationEn = Event.LocationEn;
           

            _context.tblEvents.Update(VEvent);
            _context.SaveChanges();
        }
        public void DeleteEvent(Guid id)
        {
            var Event = _context.tblEvents.FirstOrDefault(b => b.Id == id);
            if (Event != null)
            {
                Event.IsActive = false;
                _context.Update(Event);
                _context.SaveChanges();
            }
        }

    }
}
