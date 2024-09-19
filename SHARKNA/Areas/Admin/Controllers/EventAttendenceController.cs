using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SHARKNA.Controllers
{
    [Area("Admin")]

    public class EventAttendenceController : Controller
    {
        private readonly EventAttendenceDomain _eventattendenceDomain;
        private readonly EventRegistrationsDomain _EventRegistrations;
        private readonly UserDomain _UserDomain;
        private readonly SHARKNAContext _context;




        public EventAttendenceController(EventAttendenceDomain eventAttendenceDomain, EventRegistrationsDomain eventRegDomain, UserDomain userDomain, SHARKNAContext context)
        {
            _eventattendenceDomain = eventAttendenceDomain;
            _EventRegistrations = eventRegDomain;
            _UserDomain = userDomain;
            _context = context;
        }
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]

        public async Task<IActionResult> Index()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username of the logged-in user

            var events = await _eventattendenceDomain.GetTblEventsAsync();//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
            return View(events);
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]

        public async Task<IActionResult> Details(Guid id)
        {
            var eventDays = await _eventattendenceDomain.GetEventDaysByEventIdAsync(id);//نطلع معلومات الفعالية عن طريق اليوم بشكل مقسم فنستدعي الايدي الموجود بالدومين ليعرض الايام لي
            if (eventDays == null)
            {
                return NotFound();
            }

            return View(eventDays);
        }
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]

        public async Task<IActionResult> Members(Guid id, int day)
        {
            var eventTitle = await _eventattendenceDomain.GetEventTitleByIdAsync(id);
            ViewBag.EventTitle = eventTitle;
            ViewBag.Day = day;

            var eventAttendance = await _eventattendenceDomain.GetTblEventAttendenceAsync(id, day);
            return View(eventAttendance);
        }



        [HttpPost]
        public async Task<IActionResult> Members(IEnumerable<EEventAttendenceViewModel> attendances, Guid id, int day)
        {
            int check = 0;
            foreach (var attendance in attendances)
            {
                int iscount = await _eventattendenceDomain.UpdateAttendance(attendance.Id, attendance.IsAttend);
                check = check + iscount;
            }

            if (check == attendances.Count())
            {
                ViewData["Successful"] = "تم التحضير بنجاح.";
            }
            else if (check < attendances.Count() && check != 0)
            {
                ViewData["Failed"] = "حدث خطأ.";
            }
            else
            {
                ViewData["Failed"] = "حدث خطأ.";
            }

            var attendT = attendances.FirstOrDefault();

            var domaininfo = await _eventattendenceDomain.GetTblEventAttendenceAsync(id,day);


            return View(domaininfo);
            
        }





    }
}
