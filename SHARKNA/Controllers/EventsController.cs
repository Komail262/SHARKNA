using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SHARKNA.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventDomain _EventDomain;

        public EventsController(EventDomain EventDomain)
        {
            _EventDomain = EventDomain;
        }

        public IActionResult Index(string Successful = "", string Falied = "" )
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;

            else if (Falied != "")
                ViewData["Falied"] = Falied;

            var Events = _EventDomain.GettblEvents();
            return View(Events);

        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventViewModel Event)
        {


            try
            {
                if (ModelState.IsValid)
                {
                    if (Event.EventStartDate <= Event.EventEndtDate && Event.Time < Event.EndRegTime.TimeOfDay)
                    {
                        Event.Id = Guid.NewGuid();
                        int check = _EventDomain.AddEvent(Event);

                        if (check == 1)
                        {
                            ViewData["Successful"] = "تم إضافة الحدث بنجاح";
                        }
                        else
                        {
                            ViewData["Falied"] = "حدث خطأ بالإضافة";
                        }
                    }
                    else
                    {
                        if (Event.EventStartDate > Event.EventEndtDate)
                        {
                            ViewData["Falied"] = "تأكد من تاريخ الحدث";
                        }
                        else if (Event.Time >= Event.EndRegTime.TimeOfDay)
                        {
                            ViewData["Falied"] = "تأكد من الساعة";
                        }
                        return View(Event);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إضافة الحدث";
            }

            return View(Event);

        }


        public IActionResult Edit(Guid id)
        {

            var Event = _EventDomain.GetTblEventsById(id);
            if (Event == null)
            {
                return NotFound();
            }
            return View(Event);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventViewModel Event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Event.EventStartDate <= Event.EventEndtDate && Event.Time < Event.EndRegTime.TimeOfDay)
                    {
                        Event.Id = Guid.NewGuid();
                        int check = _EventDomain.AddEvent(Event);

                        if (check == 1)
                        {
                            ViewData["Successful"] = "تم تعدل الحدث بنجاح";
                        }
                        else
                        {
                            ViewData["Falied"] = "حدث خطأ تعديل";
                        }
                    }
                    else
                    {
                        if (Event.EventStartDate > Event.EventEndtDate)
                        {
                            ViewData["Falied"] = "تأكد من تاريخ الحدث";
                        }
                        else if (Event.Time >= Event.EndRegTime.TimeOfDay)
                        {
                            ViewData["Falied"] = "تأكد من الساعة";
                        }
                        _EventDomain.UpdateEvent(Event);
                        return View(Event);

                    }

                }
            }

            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ في أثناء التعديل";
            }
            return View(Event);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {


                int check = _EventDomain.DeleteEvent(id);
                if (check == 1)
                {
                    Successful = "تم حذف الحدث بنجاح";
                }

                else
                {
                    Falied = "حدث خطأ";


                }
                

            }
            catch (Exception ex)
            {
                Falied = "حدث خطأ";

            }
            return RedirectToAction(nameof(Index), new { Successful = Successful, Falied = Falied });
        
        
        }


    }
}

    
