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
            return _context.tblEvents.Select(e => new EventViewModel
            {
               
                Id = e.Id,
                EventTitleAr = e.EventTitleAr,
                EventTitleEn = e.EventTitleEn,
                EventStartDate = e.EventStartDate,
                EventEndtDate = e.EventEndtDate,
                Time = e.Time,
                EndRegTime = e.EndRegTime,
                SpeakersAr = e.SpeakersAr,
                SpeakersEn = e.SpeakersEn,
                TopicAr = e.TopicAr,
                TopicEn = e.TopicEn,
                DescriptionAr = e.DescriptionAr,
                DescriptionEn = e.DescriptionEn,
                LocationAr = e.LocationAr,
                LocationEn = e.LocationEn,
               IsActive = e.IsActive,
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

            _context.tblEvents.Update(VEvent);
            _context.SaveChanges();
        }

       
       
    }
}
