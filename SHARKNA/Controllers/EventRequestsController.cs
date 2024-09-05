using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SHARKNA.Controllers
{
    public class EventRequestsController : Controller
    {
        private readonly EventRequestsDomain _eventRequestDomain;
        private readonly EventDomain _eventDomain;

        public EventRequestsController(EventRequestsDomain eventRequestDomain, EventDomain eventDomain)
        {
            _eventRequestDomain = eventRequestDomain;
            _eventDomain = eventDomain;
        }

        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (!string.IsNullOrEmpty(Successful))
                ViewData["Successful"] = Successful;
            else if (!string.IsNullOrEmpty(Falied))
                ViewData["Falied"] = Falied;

            var eventRequests = _eventRequestDomain.GetTblEventRequests(); 
            return View(eventRequests);
        }

        public IActionResult Create()
        {
            ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventViewModel eventViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                // تحقق من أن العدد الأقصى للحضور قيمة موجبة
                if (eventViewModel.MaxAttendence <= 0)
                {
                    ViewData["Falied"] = "الحد الأقصى للحضور يجب أن يكون قيمة موجبة.";
                    ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel); // عرض النموذج مع رسالة الخطأ
                }

                // تحقق من أن تاريخ البداية ليس أكبر من تاريخ النهاية
                if (eventViewModel.EventStartDate >= eventViewModel.EventEndtDate)
                {
                    ViewData["Falied"] = "تاريخ البداية يجب أن يكون أصغر من أو يساوي تاريخ النهاية.";
                    ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel); // عرض النموذج مع رسالة الخطأ
                }

                // تحقق من أن تاريخ النهاية ليس في الماضي
                if (eventViewModel.EventEndtDate < DateTime.Now)
                {
                    ViewData["Falied"] = "تاريخ النهاية يجب أن يكون في المستقبل.";
                    ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel); // عرض النموذج مع رسالة الخطأ
                }

                //// تحقق من أن الوقت ليس متساويًا في نفس اليوم
                //if (eventViewModel.EventStartDate.Date == eventViewModel.EventEndtDate.Date && eventViewModel.EventStartDate.TimeOfDay == eventViewModel.EventEndtDate.TimeOfDay)
                //{
                //    ViewData["Falied"] = "لا يمكن أن يكون وقت البداية ووقت النهاية متساويين في نفس اليوم.";
                //    ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr", eventViewModel.BoardId);
                //    return View(eventViewModel); // عرض النموذج مع رسالة الخطأ
                //}

                // إضافة الحدث إذا كانت التحققات صحيحة
                eventViewModel.Id = Guid.NewGuid();
                int check = _eventDomain.AddEvent(eventViewModel);

                if (check == 1)
                {
                    var eventRequestViewModel = new EventRequestViewModel
                    {
                        Id = Guid.NewGuid(),
                        EventId = eventViewModel.Id,
                        BoardId = eventViewModel.BoardId,
                        RequestStatusId = Guid.Parse("93D729FA-E7FA-4EA6-BB16-038454F8C5C2"),
                        RejectionReasons = null
                    };

                    _eventRequestDomain.AddEventViewRequest(eventRequestViewModel);
                    ViewData["Successful"] = "تم إضافة الحدث والطلب بنجاح";
                }
                else
                {
                    ViewData["Falied"] = "حدث خطأ أثناء الإضافة.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = $"حدث خطأ: {ex.Message}";
            }

            ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
            return View(eventViewModel);
        }



        public IActionResult Edit(Guid id)
        {
            ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
            var eventViewModel = _eventDomain.GetTblEventsById(id);
            if (eventViewModel == null)
            {
                return NotFound();
            }
            return View(eventViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventViewModel Event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Event.MaxAttendence <= 0)
                    {
                        ViewData["Falied"] = "الحد الأقصى للحضور يجب أن يكون قيمة موجبة.";
                        ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
                        return View(Event); // عرض النموذج مع رسالة الخطأ
                    }
                    // تحقق من أن تاريخ البداية ليس أكبر من تاريخ النهاية
                    if (Event.EventStartDate >= Event.EventEndtDate)
                    {
                        ViewData["Falied"] = "تاريخ البداية يجب أن يكون أصغر من أو يساوي تاريخ النهاية.";
                        ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
                        return View(Event); // عرض النموذج مع رسالة الخطأ
                    }

                    // تحقق من أن تاريخ النهاية ليس في الماضي
                    if (Event.EventEndtDate < DateTime.Now)
                    {
                        ViewData["Falied"] = "تاريخ النهاية يجب أن يكون في المستقبل.";
                        ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
                        return View(Event); // عرض النموذج مع رسالة الخطأ
                    }

                    //if (Event.EventStartDate.Date == Event.EventEndtDate.Date && Event.EventStartDate.TimeOfDay == Event.EventEndtDate.TimeOfDay)
                    //{
                    //    ViewData["Falied"] = "لا يمكن أن يكون وقت البداية ووقت النهاية متساويين في نفس اليوم.";
                    //    ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
                    //    return View(Event); // عرض النموذج مع رسالة الخطأ
                    //}

                    int check = _eventDomain.UpdateEvent(Event); // استخدام UpdateEvent بدلاً من AddEvent لتعديل الحدث الحالي

                    if (check == 1)
                    {
                        ViewData["Successful"] = "تم تعديل الحدث بنجاح";
                    }
                    else
                    {
                        ViewData["Falied"] = "حدث خطأ أثناء التعديل";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء التعديل";
            }
            ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
            return View(Event); // عرض النموذج مرة أخرى مع رسائل الخطأ أو النجاح

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(Guid id)
        {
            try
            {
                // قم بتحديث حالة الطلب إلى "تم الإلغاء"
                _eventRequestDomain.CancelEventRequest(id);
                ViewData["Successful"] = "تم إلغاء الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إلغاء الطلب.";
            }

            // قم بجلب البيانات المحدثة بعد الإلغاء
            var eventRequests = _eventRequestDomain.GetTblEventRequests();
            return View("Index", eventRequests); // عرض View Index بعد التحديث
        }




        [HttpGet]
        public async Task<IActionResult> Accept(Guid id)
        {
            try
            {
                await _eventRequestDomain.AcceptRequest(id);
                return RedirectToAction(nameof(Index), new { Successful = "تم قبول الطلب بنجاح." });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { Falied = "حدث خطأ أثناء قبول الطلب." });
            }
        }

        // Details Action
        public IActionResult Details(Guid id)
        {
            var eventRequest = _eventRequestDomain.GetEventRequestById(id);
            if (eventRequest == null)
            {
                return NotFound();
            }
            ViewBag.RequestStatusList = new SelectList(_eventRequestDomain.GetTblRequestStatus(), "Id", "RequestStatusAr", eventRequest.RequestStatusId);
            return View(eventRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(Guid id, string rejectionReason)
        {
            try
            {
                _eventRequestDomain.RejectRequest(id, rejectionReason);
                return RedirectToAction(nameof(Index), new { Successful = "تم رفض الطلب بنجاح." });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { Falied = "حدث خطأ أثناء رفض الطلب." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRequestStatus(Guid id, Guid RequestStatusId, string RejectionReasons)
        {
            int result = _eventRequestDomain.UpdateRequestStatus(id, RequestStatusId, RejectionReasons);
            if (result == 1)
            {
                return RedirectToAction(nameof(Index), new { Successful = "تم تحديث حالة الطلب بنجاح." });
            }
            else
            {
                return RedirectToAction(nameof(Index), new { Falied = "فشل في تحديث حالة الطلب." });
            }
        }
    }
}
