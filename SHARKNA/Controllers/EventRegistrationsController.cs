using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SHARKNA.Controllers
{
    public class EventRegistrationsController : Controller
    {
        private readonly EventRegistrationsDomain _EventRegistrations;
        private readonly EventDomain _EventDomain;
        private readonly UserDomain _UserDomain;

        public EventRegistrationsController(EventRegistrationsDomain eventRegDomain, EventDomain eventDomain , UserDomain userDomain)
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

            var eventDetails = eventRegs.Select(reg => {
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
        public IActionResult Register()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var events = _EventRegistrations.GetEventsForUser(username); 
            return View(events);
        }





        [HttpPost]
        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Guid EventId)
        {
            try
            {
                
                var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

                var user = _UserDomain.GetUserFER(username);


                var EventReg = new EventRegistrationsViewModel
                {
                    Id = Guid.NewGuid(),
                    RegDate = DateTime.Now,
                    RejectionReasons = "b674955e60fd",
                    EventId = EventId,
                    UserName = username,
                    Email = user.Email,
                    MobileNumber = user.MobileNumber,
                    FullNameAr = user.FullNameAr,
                    FullNameEn = user.FullNameEn
                };

                _EventRegistrations.AddEventReg(EventReg);

            }
            catch (Exception ex)
            {
                ViewData["Failed"] = "هناك خطأ في النظام";
            }

            return RedirectToAction("Register");
        }


    }
}
