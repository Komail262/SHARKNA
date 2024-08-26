using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using SHARKNA.Models;

namespace SHARKNA.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventDomain _eventDomain;
        private readonly SHARKNAContext _context;

        public EventsController(EventDomain eventDomain, SHARKNAContext context)
        {
            _eventDomain = eventDomain;
            _context = context;
        }

        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (!string.IsNullOrEmpty(Successful))
                ViewData["Successful"] = Successful;
            else if (!string.IsNullOrEmpty(Falied))
                ViewData["Falied"] = Falied;

            var events = _eventDomain.GettblEvents();
            return View(events);
        }

        public IActionResult Create()
        {
            ViewBag.BoardsList = new SelectList(_context.tblBoards.ToList(), "Id", "NameAr");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventViewModel eventViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (eventViewModel.EventStartDate <= eventViewModel.EventEndtDate && eventViewModel.EventStartDate.TimeOfDay < eventViewModel.EventEndtDate.TimeOfDay)
                    {
                        eventViewModel.Id = Guid.NewGuid();
                        int check = _eventDomain.AddEvent(eventViewModel);

                        if (check == 1)
                        {
                            ViewData["Successful"] = "تم إضافة الحدث بنجاح";
                            return RedirectToAction(nameof(Index), new { Successful = ViewData["Successful"] });
                        }
                        else
                        {
                            ViewData["Falied"] = "حدث خطأ أثناء الإضافة";
                        }
                    }
                    else
                    {
                        if (eventViewModel.EventStartDate > eventViewModel.EventEndtDate)
                        {
                            ViewData["Falied"] = "تأكد من تاريخ الحدث";
                        }
                        else if (eventViewModel.EventStartDate.TimeOfDay >= eventViewModel.EventEndtDate.TimeOfDay)
                        {
                            ViewData["Falied"] = "تأكد من الساعة";
                        }
                        return View(eventViewModel);
                    }
                }
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إضافة الحدث";
            }

            ViewBag.BoardsList = new SelectList(_context.tblBoards.ToList(), "Id", "NameAr");
            return View(eventViewModel);
        }

        public IActionResult Edit(Guid id)
        {
            ViewBag.BoardsList = new SelectList(_context.tblBoards.ToList(), "Id", "NameAr");
            var eventViewModel = _eventDomain.GetTblEventsById(id);
            if (eventViewModel == null)
            {
                return NotFound();
            }
            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventViewModel eventViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (eventViewModel.EventStartDate <= eventViewModel.EventEndtDate && eventViewModel.EventStartDate.TimeOfDay < eventViewModel.EventEndtDate.TimeOfDay)
                    {
                        int check = _eventDomain.UpdateEvent(eventViewModel);

                        if (check == 1)
                        {
                            ViewData["Successful"] = "تم تعديل الحدث بنجاح";
                            return RedirectToAction(nameof(Index), new { Successful = ViewData["Successful"] });
                        }
                        else
                        {
                            ViewData["Falied"] = "حدث خطأ أثناء التعديل";
                        }
                    }
                    else
                    {
                        if (eventViewModel.EventStartDate > eventViewModel.EventEndtDate)
                        {
                            ViewData["Falied"] = "تأكد من تاريخ الحدث";
                        }
                        else if (eventViewModel.EventStartDate.TimeOfDay >= eventViewModel.EventEndtDate.TimeOfDay)
                        {
                            ViewData["Falied"] = "تأكد من الساعة";
                        }
                        return View(eventViewModel);
                    }
                }
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء التعديل";
            }

            ViewBag.BoardsList = new SelectList(_context.tblBoards.ToList(), "Id", "NameAr");
            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {
                int check = _eventDomain.DeleteEvent(id);
                if (check == 1)
                {
                    Successful = "تم حذف الحدث بنجاح";
                }
                else
                {
                    Falied = "حدث خطأ أثناء الحذف";
                }
            }
            catch (Exception)
            {
                Falied = "حدث خطأ أثناء الحذف";
            }
            return RedirectToAction(nameof(Index), new { Successful = Successful, Falied = Falied });
        }
    }
}
