using Microsoft.AspNetCore.Mvc;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SHARKNA.Controllers
{
    public class EventRegistrationsController : Controller
    {
        private readonly EventRegistrationsDomain _EventRegistrations;
        private readonly EventDomain _EventDomain;
        private readonly UserDomain _UserDomain;

        public EventRegistrationsController(EventRegistrationsDomain eventRegDomain, EventDomain eventDomain, UserDomain userDomain)
        {
            _EventRegistrations = eventRegDomain;
            _EventDomain = eventDomain;
            _UserDomain = userDomain;
        }


        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        public IActionResult MyEventRegistrations()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;


            var eventRegs = _EventRegistrations.GetUserRegisteredEvents(username);


            var eventDetails = eventRegs.Select(reg =>
            {
                var eventDetail = _EventDomain.GetEventById(reg.EventId);
                return new
                {
                    EventReg = reg,
                    EventDetail = eventDetail
                };
            }).ToList();

            return View(eventDetails);
        }

        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        public IActionResult MyEventDetails(Guid eventId)
        {
            var eventDetail = _EventDomain.GetEventById(eventId);
            return View(eventDetail);
        }


        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        public IActionResult Register()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var events = _EventRegistrations.GetEventsForUser(username);
            return View(events);
        }

        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        public IActionResult EventDetails(Guid eventId)
        {
            var eventDetail = _EventDomain.GetEventById(eventId);

            if (eventDetail == null)
            {
                return NotFound();
            }

            var userId = User.Identity.Name;

            var isUserRegistered = _EventRegistrations.GetUserRegisteredEvents(userId)
                .Any(r => r.EventId == eventId);

            var currentRegistrations = _EventRegistrations.GetEventRegistrationsCount(eventId);

            var maxAttendance = eventDetail.MaxAttendence;

            var canRegister = !isUserRegistered && currentRegistrations < maxAttendance;

            ViewBag.IsUserRegistered = isUserRegistered;
            ViewBag.CanRegister = canRegister;
            ViewBag.CurrentRegistrations = currentRegistrations;
            ViewBag.MaxAttendance = maxAttendance;

            return View(eventDetail);
        }






        [HttpPost]
        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Guid eventId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;


                var userRegisteredEvents = await Task.Run(() => _EventRegistrations.GetUserRegisteredEvents(username));
                var isAlreadyRegistered = userRegisteredEvents.Any(reg => reg.EventId == eventId);

                if (isAlreadyRegistered)
                {
                    ViewData["Failed"] = "أنت مسجل بالفعل في هذه الفعالية.";
                    var eventDetail = await Task.Run(() => _EventDomain.GetEventById(eventId));
                    return View("EventDetails", eventDetail);
                }


                var eventInfo = await Task.Run(() => _EventDomain.GetEventById(eventId));
                var currentRegistrationsCount = await Task.Run(() => _EventRegistrations.GetEventRegistrationsCount(eventId));

                if (currentRegistrationsCount >= eventInfo.MaxAttendence)
                {
                    ViewData["Failed"] = "لقد تم الوصول إلى الحد الأقصى للحضور.";
                    return View("EventDetails", eventInfo);
                }


                var user = await Task.Run(() => _UserDomain.GetUserFER(username));

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

                await Task.Run(() => _EventRegistrations.AddEventReg(eventReg));
                ViewData["Success"] = "تم التسجيل بنجاح!";

                var eventDetailsFinal = await Task.Run(() => _EventDomain.GetEventById(eventId));
                return View("EventDetails", eventDetailsFinal);
            }
            catch (Exception ex)
            {
 
                ViewData["Failed"] = "حدث خطأ غير متوقع في النظام.";
                var eventDetails = _EventDomain.GetEventById(eventId);
                return View("EventDetails", eventDetails);
            }
        }


    }
}
