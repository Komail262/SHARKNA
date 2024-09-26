using Microsoft.AspNetCore.Mvc;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SHARKNA.Models;

namespace SHARKNA.Controllers
{
    [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor,Club Manager")]
    public class EventRegistrationsController : Controller
    {
        private readonly EventRegistrationsDomain _eventRegistrations;
        private readonly EventDomain _eventDomain;
        private readonly UserDomain _userDomain;
        private readonly SHARKNAContext _context;
        private readonly EventRequestsDomain _eventRequestsDomain;


        public EventRegistrationsController(EventRegistrationsDomain eventRegDomain, EventDomain eventDomain, UserDomain userDomain, EventRequestsDomain eventRequestsDomain, SHARKNAContext context)
        {
            _eventRegistrations = eventRegDomain;
            _eventDomain = eventDomain;
            _userDomain = userDomain;
            _eventRequestsDomain = eventRequestsDomain;
            _context = context;
        }

        public IActionResult MyEventRegistrations()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;


            var eventRegs = _eventRegistrations.GetUserRegisteredEvents(username);


            var eventDetails = eventRegs.Select(reg =>
            {
                var eventDetail = _eventDomain.GetEventById(reg.EventId);
                return new
                {
                    EventReg = reg,
                    EventDetail = eventDetail
                };
            }).ToList();

            return View(eventDetails);
        }





        public async Task<IActionResult> MyEventDetails(Guid eventId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var nameAr = User.FindFirst(ClaimTypes.GivenName)?.Value;

            var eventDetail = await _eventDomain.GetEventByIdAsync2(eventId, username, nameAr);

            if (eventDetail == null)
            {
                return NotFound();
            }

            var userRegisteredEvents = await _eventRegistrations.GetUserRegisteredEventsAsync(username);
            var isUserRegistered = userRegisteredEvents.Any(r => r.EventId == eventId);

            var currentRegistrations = await _eventRegistrations.GetEventRegistrationsCountAsync(eventId);
            var maxAttendance = eventDetail.MaxAttendence;
            var canRegister = !isUserRegistered && currentRegistrations < maxAttendance;

            ViewBag.IsUserRegistered = isUserRegistered;
            ViewBag.CanRegister = canRegister;
            ViewBag.CurrentRegistrations = currentRegistrations;
            ViewBag.MaxAttendance = maxAttendance;

            return View(eventDetail);
        }

        public async Task<IActionResult> Register()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var nameAr = User.FindFirst(ClaimTypes.GivenName)?.Value;
            var userGender = User.FindFirst(ClaimTypes.Gender)?.Value ?? "NotSpecified";

        
            var events = await _eventDomain.GettblEventsAsync2(userGender, username, nameAr);

        
            var acceptedStatusId = Guid.Parse("11e42297-d061-42a0-b190-7d7b26936bab");


            var filteredEvents = events
                .Where(e => !e.IsDeleted && e.IsActive) 
                .Where(e => _context.tblEventRequests.Any(r => r.EventId == e.Id && r.RequestStatusId == acceptedStatusId)) 
                .ToList(); 
            return View(filteredEvents);
        }



        public async Task<IActionResult> EventDetails(Guid eventId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var nameAr = User.FindFirst(ClaimTypes.GivenName)?.Value;

            var eventDetail = await _eventDomain.GetEventByIdAsync2(eventId, username, nameAr);

            if (eventDetail == null)
            {
                return NotFound();
            }

            var userRegisteredEvents = await _eventRegistrations.GetUserRegisteredEventsAsync(username);
            var isUserRegistered = userRegisteredEvents.Any(r => r.EventId == eventId);

            var currentRegistrations = await _eventRegistrations.GetEventRegistrationsCountAsync(eventId);
            var maxAttendance = eventDetail.MaxAttendence;
            var canRegister = !isUserRegistered && currentRegistrations < maxAttendance;

            ViewBag.IsUserRegistered = isUserRegistered;
            ViewBag.CanRegister = canRegister;
            ViewBag.CurrentRegistrations = currentRegistrations;
            ViewBag.MaxAttendance = maxAttendance;

            return View(eventDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Guid eventId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            try
            {
                var userRegisteredEvents = await _eventRegistrations.GetUserRegisteredEventsAsync(username);
                var isAlreadyRegistered = userRegisteredEvents.Any(reg => reg.EventId == eventId);

                if (isAlreadyRegistered)
                {
                    ViewData["Failed"] = "أنت مسجل بالفعل في هذه الفعالية.";
                    var eventDetail = await _eventDomain.GetEventByIdAsync2(eventId, username, null);
                    return View("EventDetails", eventDetail);
                }

                var eventInfo = await _eventDomain.GetEventByIdAsync2(eventId, username, null);
                var currentRegistrationsCount = await _eventRegistrations.GetEventRegistrationsCountAsync(eventId);

                if (currentRegistrationsCount >= eventInfo.MaxAttendence)
                {
                    ViewData["Failed"] = "لقد تم الوصول إلى الحد الأقصى للحضور.";
                    return View("EventDetails", eventInfo);
                }

                var user = await _userDomain.GetUserFERAsync(username);

                var eventReg = new EventRegistrationsViewModel
                {
                    Id = Guid.NewGuid(),
                    RegDate = DateTime.Now,
                    RejectionReasons = "b674955e60fd",
                    EventId = eventId,
                    UserName = username,
                    Email = user.Email,
                    MobileNumber = user.MobileNumber,
                    FullNameAr = user.FullNameAr,
                    FullNameEn = user.FullNameEn
                };

                await _eventRegistrations.AddEventRegAsync(eventReg);

                ViewData["Success"] = "تم التسجيل بنجاح!";
                var eventDetailsFinal = await _eventDomain.GetEventByIdAsync2(eventId, username, null);
                return View("EventDetails", eventDetailsFinal);
            }
            catch
            {
                ViewData["Failed"] = "حدث خطأ غير متوقع في النظام.";
                var eventDetails = await _eventDomain.GetEventByIdAsync2(eventId, username, null);
                return View("EventDetails", eventDetails);
            }
        }
    }
}
