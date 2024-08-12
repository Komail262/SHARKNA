using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;

namespace SHARKNA.Controllers
{
    public class EventAttendenceController : Controller
    {
        private readonly EventAttendenceDomain _eventattendenceDomain;
        //private readonly EventMembersDomain _eventmembersDomain;
        private readonly EventRegistrationsDomain _EventRegistrations;


        public EventAttendenceController(EventAttendenceDomain eventAttendenceDomain, EventRegistrationsDomain eventRegDomain)
        {
            _eventattendenceDomain = eventAttendenceDomain;
            //_eventmembersDomain = eventMembersDomain;
            _EventRegistrations = eventRegDomain;
        }
        public IActionResult Index()
        {
            var events = _eventattendenceDomain.GetTblEvents();//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
            return View(events);
        }


        public IActionResult Details(Guid id)
        {
            var eventDays = _eventattendenceDomain.GetEventDaysByEventId(id);//نطلع معلومات الفعالية عن طريق اليوم بشكل مقسم فنستدعي الايدي الموجود بالدومين ليعرض الايام لي
            if (eventDays == null)
            {
                return NotFound();
            }

            return View(eventDays);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Details(EEventAttendenceViewModel atten)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    atten.Id = Guid.NewGuid();

                    int check = _eventattendenceDomain.AddEventAttend(atten);
                    if (check == 1)
                        ViewData["Successful"] = "تم تسجيل طلبك بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ";
                    return View(atten);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ";
            }

            return View(atten);
        }

        //public IActionResult Attend(Guid eventId)
        //{
        //    Guid Accepted = Guid.Parse("")


        //}
    }
}
