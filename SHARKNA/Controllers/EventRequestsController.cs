using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using SHARKNA.Domain;
using SHARKNA.ViewModels;

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
            return RedirectToAction(nameof(Index));
        }

        ViewBag.BoardsList = new SelectList(_eventRequestDomain.GetTblBoards(), "Id", "NameAr");
        ViewBag.EventsList = new SelectList(_eventRequestDomain.GetTblEventDomain(), "Id", "EventTitleAr");
        ViewBag.RequestStatusList = new SelectList(_eventRequestDomain.GetTblRequestStatus(), "Id", "RequestStatusAr");

        return View(eventRequest);
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
