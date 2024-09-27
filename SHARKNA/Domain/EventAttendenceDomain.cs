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



        public async Task<string> GetEventTitleByIdAsync(Guid eventId)
        {
            // Fetch the event title in Arabic; you can also include English if needed
            var eventEntity = await _context.tblEvents.FirstOrDefaultAsync(e => e.Id == eventId);
            return eventEntity?.EventTitleAr; // Return the Arabic title; adjust if you want the English title
        }


        public async Task<IEnumerable<EEventAttendenceViewModel>> GetTblEventAttendenceAsync(Guid eventId, int day)
        {

            return await _context.tblEventAttendence.Include(R => R.EventsReg)
                .Where(x => x.EventsId == eventId && x.Day == day)
                .Select(x => new EEventAttendenceViewModel
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

        public async Task<int> UpdateAttendance(Guid guid, bool isAttend, string username)
        {
            var attendance = await _context.tblEventAttendence.Include(x=>x.Events).FirstOrDefaultAsync(x => x.Id == guid);
            if (attendance == null)
            {
                return 0;
            }

            attendance.IsAttend = isAttend;
            _context.tblEventAttendence.Update(attendance);

            tblEventAttendenceLogs at = new tblEventAttendenceLogs();


                at.Id = Guid.NewGuid();
            at.evattend = attendance.Id;
                at.OpType = isAttend ? "حاضر" : "غائب ";
                at.OpDateTime = DateTime.Now;
                at.CreatedBy = username;
                at.CreatedTo = attendance.EventsId.ToString();
                at.AdditionalInfo = $"تم تغيير حالة الحضور لحدث {attendance.Events.EventTitleAr} إلى {(isAttend ? "حاضر" : "غائب")} بواسطة {username}.";
            

            _context.tblEventAttendenceLogs.Add(at);

            return await _context.SaveChangesAsync();


        }



        //===================================================================================================
        public async Task<IEnumerable<EventViewModel>> GetArchivedEventsAsync()
        {
            return await _context.tblEvents
                .Where(e => e.EventEndtDate < DateTime.Now ) // Only events that have ended
                .Select(x => new EventViewModel
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



    }
}
