using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System;

public class EventRequestsController : Controller
{
    private readonly EventRequestsDomain _eventRequestDomain;
    private readonly BoardDomain _BoardDomain;
    private readonly EventDomain _EventDomain;
    private readonly RequestStatusDomain _RequestStatusDomain;

    public EventRequestsController(EventRequestsDomain eventRequestDomain, BoardDomain boardDomain, EventDomain eventDomain, RequestStatusDomain requestStatusDomain)
    {
        _eventRequestDomain = eventRequestDomain;
        _BoardDomain = boardDomain;
        _EventDomain = eventDomain;
        _RequestStatusDomain = requestStatusDomain;
    }

    public IActionResult Index()
    {
        var eventRequests = _eventRequestDomain.GetTblEventRequests();
        return View(eventRequests);
    }

    public IActionResult Create()
    {
        ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
        ViewBag.EventsList = new SelectList(_eventRequestDomain.GetTblEventDomain(), "Id", "EventTitleAr");
        ViewBag.RequestStatusList = new SelectList(_eventRequestDomain.GetTblRequestStatus(), "Id", "RequestStatusAr");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EventRequestViewModel eventRequest)
    {
        if (ModelState.IsValid)
        {
            eventRequest.Id = Guid.NewGuid();
            _eventRequestDomain.AddEventViewRequest(eventRequest);
            ViewData["Successful"] = "تم تسجيل طلبك بنجاح";
            return View(eventRequest);
        }

        ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
        ViewBag.EventsList = new SelectList(_eventRequestDomain.GetTblEventDomain(), "Id", "EventTitleAr");
        ViewBag.RequestStatusList = new SelectList(_eventRequestDomain.GetTblRequestStatus(), "Id", "RequestStatusAr");

        return View(eventRequest);
    }

    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var request = _eventRequestDomain.GetEventRequestById(id);
        if (request == null)
        {
            return NotFound();
        }

        ViewBag.RequestStatusList = new SelectList(_eventRequestDomain.GetTblRequestStatus(), "Id", "RequestStatusAr");
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateRequestStatus(Guid id, Guid RequestStatusId, string RejectionReasons)
    {
        try
        {
            var request = _eventRequestDomain.GetEventRequestById(id);
            if (request == null)
            {
                ViewData["Failed"] = "الطلب غير موجود";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            if (ModelState.IsValid)
            {
                int check = _eventRequestDomain.UpdateRequestStatus(id, RequestStatusId, RejectionReasons);
                if (check == 1)
                {
                    ViewData["Successful"] = "تم تحديث حالة الطلب بنجاح";
                }
                else
                {
                    ViewData["Failed"] = "حدث خطأ أثناء تحديث حالة الطلب";
                    ViewData["NoRedirect"] = true; // التحكم بعدم إعادة التوجيه
                }
            }
        }
        catch (Exception)
        {
            ViewData["Failed"] = "حدث خطأ أثناء محاولة تحديث حالة الطلب";
            ViewData["NoRedirect"] = true; // التحكم بعدم إعادة التوجيه
        }

        return RedirectToAction(nameof(Details), new { id = id });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancel(Guid id)
    {
        try
        {
            _eventRequestDomain.CancelEventRequest(id);
            TempData["Success"] = "تم إلغاء الطلب بنجاح";
        }
        catch (Exception)
        {
            TempData["Error"] = "حدث خطأ أثناء محاولة إلغاء الطلب";
        }

        return RedirectToAction(nameof(Index));
    }
}
