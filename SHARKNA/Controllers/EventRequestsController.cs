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
        try
        {
            ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr", eventRequest.BoardId);
            ViewBag.EventsList = new SelectList(_eventRequestDomain.GetTblEventDomain(), "Id", "EventTitleAr", eventRequest.EventId);
            ViewBag.RequestStatusList = new SelectList(_eventRequestDomain.GetTblRequestStatus(), "Id", "RequestStatusAr", eventRequest.RequestStatusId);

            if (ModelState.IsValid)
            {
                eventRequest.Id = Guid.NewGuid();
                _eventRequestDomain.AddEventViewRequest(eventRequest);
                ViewData["Successful"] = "تم تسجيل طلبك بنجاح";
                return View(eventRequest);
            }
        }
        catch (Exception ex)
        {
            ViewData["Falied"] = "حدث خطأ أثناء معالجة الطلب";
        }

        return View(eventRequest);
    }
}
