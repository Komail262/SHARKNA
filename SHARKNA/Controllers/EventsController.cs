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
            if (ModelState.IsValid)
            {
                Event.Id =Guid.NewGuid();
                _EventDomain.AddEvent(Event);
                return RedirectToAction(nameof(Index));
            }
            return View();
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
            if (ModelState.IsValid)
            {
                _EventDomain.UpdateEvent(Event);

                return RedirectToAction(nameof(Index));
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
                    Successful = "تم حذف  بنجاح";
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
            //_boardDomain.DeleteBoard(id);
            return RedirectToAction(nameof(Index), new { Successful = Successful, Falied = Falied });
        
        
        }


    }
}

    
