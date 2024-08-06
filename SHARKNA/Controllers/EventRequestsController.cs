using Microsoft.AspNetCore.Mvc;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System;

namespace SHARKNA.Controllers
{
    public class EventRequestsController : Controller
    {
        private readonly EventRequestsDomain _eventRequestDomain;

        public EventRequestsController(EventRequestsDomain eventRequestDomain)
        {
            _eventRequestDomain = eventRequestDomain;
        }

        public IActionResult Index()
        {
            var eventRequests = _eventRequestDomain.GetTblEventRequests();
            return View(eventRequests);
        }

        // إضافة إجراء إلغاء الطلب
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelRequest(Guid id)
        {
            try
            {
                _eventRequestDomain.CancelRequest(id);
                TempData["Message"] = "تم إلغاء الطلب بنجاح."; // رسالة نجاح
            }
            catch (Exception)
            {
                TempData["Error"] = "حدث خطأ أثناء محاولة إلغاء الطلب."; // رسالة خطأ
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
