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

        public IActionResult Index(string Successful = "", string Falied = "")
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
                    if (Event.EventStartDate >= Event.EventEndtDate)
                    {
                        ModelState.AddModelError("", "تاريخ البداية يجب أن يكون أصغر من تاريخ النهاية.");
                        return View(Event);
                    }


                    if (Event.Time >= Event.EndRegTime.TimeOfDay)
                    {
                        ModelState.AddModelError("", "بداية الساعة يجب أن تكون أصغر من نهاية الساعة.");
                        return View(Event);
                    }

                    Event.Id = Guid.NewGuid();

                    int check = _EventDomain.AddEvent(Event);
                    if (check == 1)
                        ViewData["Successful"] = "تم إضافة الحدث بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ بالإضافة";
                    return View(Event);

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

                    if (Event.EventStartDate >= Event.EventEndtDate)
                    {
                        ModelState.AddModelError("", "تاريخ البداية يجب أن يكون أصغر من تاريخ النهاية.");
                        return View(Event);
                    }


                    if (Event.Time >= Event.EndRegTime.TimeOfDay)
                    {
                        ModelState.AddModelError("", "بداية الساعة يجب أن تكون أصغر من نهاية الساعة.");
                        return View(Event);
                    }

                    int check = _EventDomain.UpdateEvent(Event);
                    if (check == 1)
                        ViewData["Successful"] = "تم التعديل بنجاح";
                    else
                        ViewData["Falied"] = "خطأ بالتعديل";
                    return View(Event);

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

    
