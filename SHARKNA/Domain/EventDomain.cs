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
                    BoardId = e.BoardId,
                    Gender = e.Gender,
                    BoardName = e.Board.NameAr
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
                EventStartDate = (existingEvent.EventStartDate.Year >= 1900 && existingEvent.EventStartDate.Year <= 2077) ? existingEvent.EventStartDate : (DateTime?)null,
                EventEndtDate = (existingEvent.EventEndtDate.Year >= 1900 && existingEvent.EventEndtDate.Year <= 2077) ? existingEvent.EventEndtDate : (DateTime?)null,
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
                BoardId = existingEvent.BoardId,
                Gender = existingEvent.Gender,
                BoardName = existingEvent.Board.NameAr,
            };
        }


        public int AddEvent(EventViewModel eventViewModel)
        {
            try
            {
                // التحقق من وجود قيم لتواريخ البداية والنهاية
                if (!eventViewModel.EventStartDate.HasValue || !eventViewModel.EventEndtDate.HasValue)
                {
                    // إذا لم تكن هناك قيم صالحة، نعيد 0 أو أي كود خطأ مناسب
                    return 0;
                }

                var newEvent = new tblEvents
                {
                    Id = eventViewModel.Id,
                    EventTitleAr = eventViewModel.EventTitleAr,
                    EventTitleEn = eventViewModel.EventTitleEn,
                    EventStartDate = eventViewModel.EventStartDate.Value, // استخدام .Value بعد التحقق من أنها ليست فارغة
                    EventEndtDate = eventViewModel.EventEndtDate.Value, // استخدام .Value بعد التحقق من أنها ليست فارغة
                    SpeakersAr = eventViewModel.SpeakersAr,
                    SpeakersEn = eventViewModel.SpeakersEn,
                    TopicAr = eventViewModel.TopicAr,
                    TopicEn = eventViewModel.TopicEn,
                    DescriptionAr = eventViewModel.DescriptionAr,
                    DescriptionEn = eventViewModel.DescriptionEn,
                    LocationAr = eventViewModel.LocationAr,
                    LocationEn = eventViewModel.LocationEn,
                    Gender = eventViewModel.Gender,
                    MaxAttendence = eventViewModel.MaxAttendence,
                    IsActive = true,
                    IsDeleted = false,
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

                // التحقق من صحة تاريخ البدء والنهاية
                if (eventViewModel.EventStartDate.HasValue &&
                    eventViewModel.EventStartDate.Value.Year >= 1900 &&
                    eventViewModel.EventStartDate.Value.Year <= 2077)
                {
                    existingEvent.EventStartDate = eventViewModel.EventStartDate.Value;
                }

                if (eventViewModel.EventEndtDate.HasValue &&
                    eventViewModel.EventEndtDate.Value.Year >= 1900 &&
                    eventViewModel.EventEndtDate.Value.Year <= 2077)
                {
                    existingEvent.EventEndtDate = eventViewModel.EventEndtDate.Value;
                }

                existingEvent.SpeakersAr = eventViewModel.SpeakersAr;
                existingEvent.SpeakersEn = eventViewModel.SpeakersEn;
                existingEvent.TopicAr = eventViewModel.TopicAr;
                existingEvent.TopicEn = eventViewModel.TopicEn;
                existingEvent.DescriptionAr = eventViewModel.DescriptionAr;
                existingEvent.DescriptionEn = eventViewModel.DescriptionEn;
                existingEvent.LocationAr = eventViewModel.LocationAr;
                existingEvent.LocationEn = eventViewModel.LocationEn;
                existingEvent.MaxAttendence = eventViewModel.MaxAttendence;
                existingEvent.Gender = eventViewModel.Gender;
                existingEvent.IsDeleted = false;
                existingEvent.IsActive = true;
                existingEvent.BoardId = eventViewModel.BoardId;

                _context.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        } 



        //public int DeleteEvent(Guid id)
        //{
        //    try
        //    {
        //        var existingEvent = _context.tblEvents.FirstOrDefault(e => e.Id == id);
        //        if (existingEvent == null)
        //            return 0;

        //        existingEvent.IsDeleted = true;
        //        existingEvent.IsActive = false;

        //        _context.Update(existingEvent);
        //        _context.SaveChanges();
        //        return 1;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}
    }
}
