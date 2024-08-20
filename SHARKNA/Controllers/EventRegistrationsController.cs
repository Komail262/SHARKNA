using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SHARKNA.Controllers
{
    public class EventRegistrationsController : Controller
    {
        private readonly EventRegistrationsDomain _EventRegistrations;

        public EventRegistrationsController(EventRegistrationsDomain eventRegDomain)
        {
            _EventRegistrations = eventRegDomain;

        }

        public IActionResult Index()
        {
            var EventReg = _EventRegistrations.GettblEventRegistrations();
            return View(EventReg);
        }
       
        


        // GET: EventRegistrations/Create
        public IActionResult Create()
        {
        
            ViewBag.EventsOfList = new SelectList(_EventRegistrations.GettblEvents(), "Id", "EventTitleAr");
            return View();
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventRegistrationsViewModel EventReg)
        {
           
            try
            {
                if (ModelState.IsValid)
                {
                    if (_EventRegistrations.IsEmailDuplicate(EventReg.Email))
                    {
                        ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل.");
                        ViewBag.EventsOfList = new SelectList(_EventRegistrations.GettblEvents(), "Id", "EventTitleAr");
                        return View(EventReg);
                    }

                    EventReg.Id = Guid.NewGuid();
                    EventReg.RegDate = DateTime.Now;
                    EventReg.RejectionReasons = "";
                    _EventRegistrations.AddEventReg(EventReg);
                    ViewData["Successful"] = "تمت التسجيل بنجاح";
                }
                else
                {
                    ViewData["Falied"] = "حدث خطأ أثناء معالجة طلبك , الرجاء اعادة المحاولة   ";
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "هناك خطا في النظام";
            }

            ViewBag.EventsOfList = new SelectList(_EventRegistrations.GettblEvents(), "Id", "EventTitleAr");
            return View();
        }


    }
}
