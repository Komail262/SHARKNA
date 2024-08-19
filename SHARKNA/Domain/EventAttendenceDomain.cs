using Microsoft.Extensions.Logging;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Xml;

namespace SHARKNA.Domain
{
    public class EventAttendenceDomain
    {
        private readonly SHARKNAContext _context;
        public EventAttendenceDomain(SHARKNAContext context)
        {
            _context = context;
        }
        public IEnumerable<EventViewModel> GetTblEvents()
        {
            return _context.tblEvents.Select(x => new EventViewModel //استرجع جميع قيم الفعاليات من الداتابيس واحطهم في الايفنت فيو موديل
            {
                Id = x.Id,
                EventTitleAr = x.EventTitleAr,
                EventTitleEn = x.EventTitleEn,
                EventStartDate = x.EventStartDate,
                EventEndtDate = x.EventEndtDate,
                Time = x.Time,
                EndRegTime = x.EndRegTime,
                SpeakersAr = x.SpeakersAr,
                SpeakersEn = x.SpeakersEn,
                TopicAr = x.TopicAr,
                TopicEn = x.TopicEn,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                LocationAr = x.LocationAr,
                LocationEn = x.LocationEn,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            }).ToList();
        }



        public IEnumerable<EEventAttendenceViewModel> GetEventDaysByEventId(Guid eventId)
        {
            var EE = _context.tblEvents.FirstOrDefault(e => e.Id == eventId); // نسترجع بيانات الفعاليات  من الداتابيس عن طريق الايفنت أي دي
            if (EE == null) // اذا الفعالية غير موجودة عطني غير موجودة
            {
                return null;
            }

            var days = new List<EEventAttendenceViewModel>(); // هنا نسوي لست لأيام الحدث
            var currentDate = EE.EventStartDate; // تاريخ بداية الحدث
            int dayNumber = 1; // يبدأ باليوم رقم 1

            while (currentDate <= EE.EventEndtDate) // نسوي وايل لوب من بداية الحدث الى نهايته
            {
                days.Add(new EEventAttendenceViewModel
                {
                    Id = Guid.NewGuid(), // نعمل اي دي خاص لكل يوم
                    EventstId = eventId, // نعرف الحدث
                    EventDate = currentDate, // نعرف التاريخ الحالي
                    Day = dayNumber, // نعرف رقم اليوم
                    EventName = EE.EventTitleAr // نضع عنوان الفعالية
                });

                currentDate = currentDate.AddDays(1); // يروح لليوم اللي بعده
                dayNumber++; // كل يوم يزيد +1
            }

            return days; // قائمة الايام
        }




        // new list


        //public IEnumerable<EventRegistrationsViewModel> GetTblEventRegistrations(Guid eventId , Guid Accepted)
        //{
        //    return _context.tblEventRegistrations
        //        .Where(x => x.EventsId == eventId && x.EventStatusId == Accepted)
        //        .Select(x => new EventRegistrationsViewModel //استرجع جميع قيم المسجلين من الداتابيس واحطهم في الايفنت فيو موديل

        //    {
        //        Id = x.Id,
        //        RegDate = x.RegDate,
        //        RejectionReasons = x.RejectionReasons,
        //        UserName = x.UserName,
        //        Email = x.Email,
        //        MobileNumber = x.MobileNumber,
        //        FullNameAr = x.FullNameAr,
        //        FullNameEn = x.FullNameEn,
        //        EventId = x.EventsId,
        //        RequestStatusId = x.EventStatusId

        //    }).ToList();

        //}


        public IEnumerable<EventRegistrationsViewModel> GetTblEventreg(Guid eventId)
        {
            return _context.tblEventRegistrations.Where(x => x.EventsId == eventId).Select(x => new EventRegistrationsViewModel //استرجع جميع قيم الفعاليات من الداتابيس واحطهم في الايفنت فيو موديل
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
        }



        public int AddEventAttend(EEventAttendenceViewModel eventatten)
        {
            try
            {
                var At = new tblEventAttendence
                {
                    Id = eventatten.Id,
                    EventMemId = eventatten.EventMemId,
                    EventstId = Guid.Parse(" "),
                    EventDate = eventatten.EventDate,
                    Day = eventatten.Day,
                    IsAttend = eventatten.IsAttend
                };

                _context.tblEventAttendence.Add(At);
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
