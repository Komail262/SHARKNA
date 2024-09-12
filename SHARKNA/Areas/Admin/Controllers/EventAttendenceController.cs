using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var user = _UserDomain.GetUserFER(username); // Fetch user details from the database

            var model = new BoardRequestsViewModel
            {
                UserName = username,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                FullNameAr = user.FullNameAr,
                FullNameEn = user.FullNameEn
            };
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

        public async Task<IActionResult> Members(Guid id, Guid EventsRegId)
        {
            var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, EventsRegId);//ناخذ قائمة المسجلين يالفعاليات من الدومين ايفنت اتيندينس 

            return View(Ereg);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Members(FormCollection forms, Guid id, Guid eventRegId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var eventId = new Guid(forms["eventId"]);
                    var username = User.FindFirst(ClaimTypes.Name)?.Value;
                    var user = _UserDomain.GetUserFER(username);


                    var count = forms["attendanceStatus"].Count;



                    var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, eventRegId);//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
                                                                                                       //return View(Ereg);

                    if (count == 1)
                        ViewData["Successful"] = "تم تسجيل الحضور بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ";

                    return RedirectToAction("Index");

                }
            }

            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ";
            }

            return RedirectToAction("Index");

        }


    }
}