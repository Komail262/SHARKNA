using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SHARKNA.Controllers
{
    public class EventRequestsController : Controller
    {
        private readonly EventRequestsDomain _eventRequestDomain;
        private readonly EventDomain _eventDomain;
        private readonly UserDomain _UserDomain;

        public EventRequestsController(EventRequestsDomain eventRequestDomain, EventDomain eventDomain, UserDomain userDomain)
        {
            _eventRequestDomain = eventRequestDomain;
            _eventDomain = eventDomain;
            _UserDomain = userDomain;
        }

        public async Task<IActionResult> Index(string Successful = "", string Falied = "")
        {
            if (!string.IsNullOrEmpty(Successful))
                ViewData["Successful"] = Successful;
            else if (!string.IsNullOrEmpty(Falied))
                ViewData["Falied"] = Falied;

            var eventRequests = await _eventRequestDomain.GetTblEventRequestsAsync();
            return View(eventRequests);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                if (eventViewModel.MaxAttendence <= 0)
                {
                    ViewData["Falied"] = "الحد الأقصى للحضور يجب أن يكون قيمة موجبة.";
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                if (eventViewModel.EventStartDate >= eventViewModel.EventEndtDate)
                {
                    ViewData["Falied"] = "تاريخ البداية يجب أن يكون أصغر من أو يساوي تاريخ النهاية.";
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                if (eventViewModel.EventEndtDate < DateTime.Now)
                {
                    ViewData["Falied"] = "تاريخ النهاية يجب أن يكون في المستقبل.";
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                eventViewModel.Id = Guid.NewGuid();
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                int check = await _eventDomain.AddEventAsync(eventViewModel, username);

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

                    await _eventRequestDomain.AddEventViewRequestAsync(eventRequestViewModel, username);
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

            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
            return View(eventViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
            var eventViewModel = await _eventDomain.GetTblEventsByIdAsync(id);
            if (eventViewModel == null)
            {
                return NotFound();
            }
            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel Event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Event.MaxAttendence <= 0)
                    {
                        ViewData["Falied"] = "الحد الأقصى للحضور يجب أن يكون قيمة موجبة.";
                        ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
                        return View(Event);
                    }

                    if (Event.EventStartDate >= Event.EventEndtDate)
                    {
                        ViewData["Falied"] = "تاريخ البداية يجب أن يكون أصغر من أو يساوي تاريخ النهاية.";
                        ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
                        return View(Event);
                    }

                    if (Event.EventEndtDate < DateTime.Now)
                    {
                        ViewData["Falied"] = "تاريخ النهاية يجب أن يكون في المستقبل.";
                        ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
                        return View(Event);
                    }

                    string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                    int check = await _eventDomain.UpdateEventAsync(Event, username);

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

            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
            return View(Event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                await _eventRequestDomain.CancelEventRequestAsync(id, username);
                ViewData["Successful"] = "تم إلغاء الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إلغاء الطلب.";
            }

            var eventRequests = await _eventRequestDomain.GetTblEventRequestsAsync();
            return View("Index", eventRequests);
        }

        [HttpGet]
        public async Task<IActionResult> Accept(Guid id)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                await _eventRequestDomain.AcceptRequestAsync(id, username);
                return RedirectToAction(nameof(Index), new { Successful = "تم قبول الطلب بنجاح." });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { Falied = "حدث خطأ أثناء قبول الطلب." });
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var eventRequest = await _eventRequestDomain.GetEventRequestByIdAsync(id);
            if (eventRequest == null)
            {
                return NotFound();
            }
            ViewBag.RequestStatusList = new SelectList(await _eventRequestDomain.GetTblRequestStatusAsync(), "Id", "RequestStatusAr", eventRequest.RequestStatusId);
            return View(eventRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id, string rejectionReason)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                if (username != null)
                {
                    await _eventRequestDomain.RejectRequestAsync(id, rejectionReason, username);
                    return RedirectToAction(nameof(Index), new { Successful = "تم رفض الطلب بنجاح." });
                }
                else
                {
                    return RedirectToAction(nameof(Index), new { Failed = "فشل في الحصول على اسم المستخدم." });
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { Failed = "حدث خطأ أثناء رفض الطلب." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRequestStatus(Guid id, Guid RequestStatusId, string RejectionReasons)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                if (username != null)
                {
                    int result = await _eventRequestDomain.UpdateRequestStatusAsync(id, RequestStatusId, RejectionReasons, username);
                    if (result == 1)
                    {
                        return RedirectToAction(nameof(Index), new { Successful = "تم تحديث حالة الطلب بنجاح." });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index), new { Failed = "فشل في تحديث حالة الطلب." });
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index), new { Failed = "فشل في الحصول على اسم المستخدم." });
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { Failed = "حدث خطأ أثناء تحديث حالة الطلب." });
            }
        }
    }
}
    