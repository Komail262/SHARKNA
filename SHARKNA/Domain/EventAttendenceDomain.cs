using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Xml;


namespace SHARKNA.Domain
{
    public class EventAttendenceDomain
    {
        private readonly SHARKNAContext _context;
        private readonly UserDomain _userDomain;

        public EventAttendenceDomain(SHARKNAContext context, UserDomain userDomain)
        {
            _context = context;
            _userDomain = userDomain;
        }
        public async Task<IEnumerable<EventViewModel>> GetTblEventsAsync()
        {
            return await _context.tblEvents.Select(x => new EventViewModel //استرجع جميع قيم الفعاليات من الداتابيس واحطهم في الايفنت فيو موديل
            {
                Id = x.Id,
                EventTitleAr = x.EventTitleAr,
                EventTitleEn = x.EventTitleEn,
                EventStartDate = x.EventStartDate,
                EventEndtDate = x.EventEndtDate,
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
            }).ToListAsync();
        }





        public async Task<IEnumerable<EEventAttendenceViewModel>> GetEventDaysByEventIdAsync(Guid eventId)
        {
            var EE = await _context.tblEvents.FirstOrDefaultAsync(e => e.Id == eventId); // نسترجع بيانات الفعاليات  من الداتابيس عن طريق الايفنت أي دي
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
                    EventsId = eventId, // نعرف الحدث
                    EventDate = currentDate, // نعرف التاريخ الحالي
                    Day = dayNumber, // نعرف رقم اليوم
                    EventName = EE.EventTitleAr // نضع عنوان الفعالية
                });

                currentDate = currentDate.AddDays(1); // يروح لليوم اللي بعده
                dayNumber++; // كل يوم يزيد +1
            }

            return days; // قائمة الايام
        }


        public async Task<IEnumerable<EEventAttendenceViewModel>> GetTblEventattendenceAsync(Guid eventId, int day)
        {

            return await _context.tblEventAttendence.Include(R => R.EventsReg)

                .Where(x => x.EventsId == eventId && x.Day == day).Select(x => new EEventAttendenceViewModel
                {
                    Id = x.Id,
                    EventDate = x.EventDate,
                    Day = x.Day,
                    EventsId = x.EventsId,
                    EventsRegId = x.EventsRegId,
                    NameAr = x.EventsReg.FullNameAr,
                    RRegDate = x.EventsReg.RegDate,
                    EEmail = x.EventsReg.Email,
                    IsAttend = x.IsAttend

                }).ToListAsync();


        }




        //public void IsAtten(Guid id)
        //{
        //    var att = _context.tblEventAttendence.FirstOrDefault(x => x.Id == id);

        //    if (att == null) 
        //    {
        //        att.IsAttend = false;
        //    }
        //    else
        //    {
        //        att.IsAttend = true;
        //    }

        //    _context.SaveChanges();

        //}

        public void IsAtten(Guid id)
        {
            var att = _context.tblEventAttendence.FirstOrDefault(x => x.Id == id);

            if (att != null) // Only update if att is not null
            {
                att.IsAttend = !att.IsAttend; // Toggle the IsAttend status
                _context.tblEventAttendence.Update(att);
                _context.SaveChanges();
            }
        }

        public void SetAttendanceStatus(Guid id, bool isAttending)
        {
            var att = _context.tblEventAttendence.FirstOrDefault(x => x.Id == id);

            if (att != null)
            {
                att.IsAttend = isAttending;
                _context.tblEventAttendence.Update(att); // Ensure that the entity is marked as modified
                _context.SaveChanges(); // Save changes to the database
            }
        }







    }
}
