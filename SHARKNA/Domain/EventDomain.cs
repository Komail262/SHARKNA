using System;
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
            return _context.tblEvents
                .Where(e => !e.IsDeleted)
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    EventTitleAr = e.EventTitleAr,
                    EventTitleEn = e.EventTitleEn,
                    EventStartDate = e.EventStartDate,
                    EventEndtDate = e.EventEndtDate,
                    SpeakersAr = e.SpeakersAr,
                    SpeakersEn = e.SpeakersEn,
                    TopicAr = e.TopicAr,
                    TopicEn = e.TopicEn,
                    DescriptionAr = e.DescriptionAr,
                    DescriptionEn = e.DescriptionEn,
                    LocationAr = e.LocationAr,
                    LocationEn = e.LocationEn,
                    MaxAttendence = e.MaxAttendence,
                    IsActive = e.IsActive,
                    IsDeleted = e.IsDeleted,
                    BoardId = e.BoardId
                })
                .ToList();
        }

        public EventViewModel GetTblEventsById(Guid id)
        {
            var existingEvent = _context.tblEvents.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null)
                return null;

            return new EventViewModel
            {
                Id = existingEvent.Id,
                EventTitleAr = existingEvent.EventTitleAr,
                EventTitleEn = existingEvent.EventTitleEn,
                EventStartDate = existingEvent.EventStartDate,
                EventEndtDate = existingEvent.EventEndtDate,
                SpeakersAr = existingEvent.SpeakersAr,
                SpeakersEn = existingEvent.SpeakersEn,
                TopicAr = existingEvent.TopicAr,
                TopicEn = existingEvent.TopicEn,
                DescriptionAr = existingEvent.DescriptionAr,
                DescriptionEn = existingEvent.DescriptionEn,
                LocationAr = existingEvent.LocationAr,
                LocationEn = existingEvent.LocationEn,
                MaxAttendence = existingEvent.MaxAttendence,
                IsActive = existingEvent.IsActive,
                IsDeleted = existingEvent.IsDeleted,
                BoardId = existingEvent.BoardId
            };
        }

        public int AddEvent(EventViewModel eventViewModel)
        {
            try
            {
                var newEvent = new tblEvents
                {
                    Id = eventViewModel.Id,
                    EventTitleAr = eventViewModel.EventTitleAr,
                    EventTitleEn = eventViewModel.EventTitleEn,
                    EventStartDate = eventViewModel.EventStartDate,
                    EventEndtDate = eventViewModel.EventEndtDate,
                    SpeakersAr = eventViewModel.SpeakersAr,
                    SpeakersEn = eventViewModel.SpeakersEn,
                    TopicAr = eventViewModel.TopicAr,
                    TopicEn = eventViewModel.TopicEn,
                    DescriptionAr = eventViewModel.DescriptionAr,
                    DescriptionEn = eventViewModel.DescriptionEn,
                    LocationAr = eventViewModel.LocationAr,
                    LocationEn = eventViewModel.LocationEn,
                    MaxAttendence = eventViewModel.MaxAttendence,
                    IsActive = eventViewModel.IsActive,
                    IsDeleted = eventViewModel.IsDeleted,
                    BoardId = eventViewModel.BoardId
                };

                _context.tblEvents.Add(newEvent);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int UpdateEvent(EventViewModel eventViewModel)
        {
            try
            {
                var existingEvent = _context.tblEvents.FirstOrDefault(e => e.Id == eventViewModel.Id);
                if (existingEvent == null)
                    return 0;

                existingEvent.EventTitleAr = eventViewModel.EventTitleAr;
                existingEvent.EventTitleEn = eventViewModel.EventTitleEn;
                existingEvent.EventStartDate = eventViewModel.EventStartDate;
                existingEvent.EventEndtDate = eventViewModel.EventEndtDate;
                existingEvent.SpeakersAr = eventViewModel.SpeakersAr;
                existingEvent.SpeakersEn = eventViewModel.SpeakersEn;
                existingEvent.TopicAr = eventViewModel.TopicAr;
                existingEvent.TopicEn = eventViewModel.TopicEn;
                existingEvent.DescriptionAr = eventViewModel.DescriptionAr;
                existingEvent.DescriptionEn = eventViewModel.DescriptionEn;
                existingEvent.LocationAr = eventViewModel.LocationAr;
                existingEvent.LocationEn = eventViewModel.LocationEn;
                existingEvent.MaxAttendence = eventViewModel.MaxAttendence;
                existingEvent.IsDeleted = eventViewModel.IsDeleted;
                existingEvent.IsActive = eventViewModel.IsActive;
                existingEvent.BoardId = eventViewModel.BoardId;

                _context.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int DeleteEvent(Guid id)
        {
            try
            {
                var existingEvent = _context.tblEvents.FirstOrDefault(e => e.Id == id);
                if (existingEvent == null)
                    return 0;

                existingEvent.IsDeleted = true;
                existingEvent.IsActive = false;

                _context.Update(existingEvent);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
