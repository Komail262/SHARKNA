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
    public class EventAttendenceController : Controller
    {
        private readonly EventAttendenceDomain _eventattendenceDomain;
        private readonly EventRegistrationsDomain _EventRegistrations;
        private readonly UserDomain _UserDomain;


        public EventAttendenceController(EventAttendenceDomain eventAttendenceDomain, EventRegistrationsDomain eventRegDomain, UserDomain userDomain)
        {
            _eventattendenceDomain = eventAttendenceDomain;
            _EventRegistrations = eventRegDomain;
            _UserDomain = userDomain;
        }
        public async Task<IActionResult> Index()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username of the logged-in user

            var events = await _eventattendenceDomain.GetTblEventsAsync();//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
            return View(events);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var eventDays = await _eventattendenceDomain.GetEventDaysByEventIdAsync(id);//نطلع معلومات الفعالية عن طريق اليوم بشكل مقسم فنستدعي الايدي الموجود بالدومين ليعرض الايام لي
            if (eventDays == null)
            {
                return NotFound();
            }

            return View(eventDays);
        }

        public async Task<IActionResult> Members(Guid id, int day)
        {
            var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);//ناخذ قائمة المسجلين يالفعاليات من الدومين ايفنت اتيندينس 

            return View(Ereg);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Members(FormCollection forms, Guid id, int day)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var eventId = new Guid(forms["eventId"]);
                    var username = User.FindFirst(ClaimTypes.Name)?.Value;
                    //var user = _UserDomain.GetUserFER(username);


                    var count = forms["attendanceStatus"].Count;



                    var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
                                                                                                //return View(Ereg);

                    if (count == 1)
                        ViewData["Successful"] = "تم التحضير بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ";

                    return View(forms);

                }
            }

            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ";
            }

            return View(forms);

        }


    }
}