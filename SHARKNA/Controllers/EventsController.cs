using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.Extensions.Logging;

namespace SHARKNA.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventDomain _EventDomain;

        public EventsController(EventDomain EventDomain)
        {
            _EventDomain = EventDomain;
        }

        public IActionResult Index()
        {
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
            _EventDomain.DeleteEvent(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

    
