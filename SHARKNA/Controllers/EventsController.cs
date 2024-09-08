//using Microsoft.AspNetCore.Mvc;
//using SHARKNA.Models;
//using SHARKNA.Domain;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using SHARKNA.ViewModels;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace SHARKNA.Controllers
//{
//    public class EventsController : Controller
//    {
//        private readonly EventDomain _EventDomain;
//        private readonly BoardDomain _boardDomain;

//        public EventsController(EventDomain EventDomain, BoardDomain boardDomain)
//        {
//            _EventDomain = EventDomain;
//            _boardDomain = boardDomain;
//        }

//        public IActionResult Index(string Successful = "", string Falied = "" )
//        {
//            if (Successful != "")
//                ViewData["Successful"] = Successful;

//            else if (Falied != "")
//                ViewData["Falied"] = Falied;

//            var Events = _EventDomain.GettblEvents();
//            return View(Events);

//        }


//        public IActionResult Create()
//        {
//            return View();
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(EventViewModel Event)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    int check = _EventDomain.AddEvent(Event);
//                    if (check == 1)
//                    {
//                        ViewData["Successful"] = "تم إضافة الحدث بنجاح";
//                        return RedirectToAction("Index", "EventRequests");
//                    }
//                    else
//                    {
//                        ViewData["Falied"] = "حدث خطأ أثناء الإضافة";
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewData["Falied"] = "حدث خطأ أثناء الإضافة";
//            }

//            return View(Event); // إعادة عرض النموذج إذا كان هناك خطأ
//        }



        //public IActionResult Edit(Guid id)
        //{
        //    ViewBag.BoardsList = new SelectList(_boardDomain.GetTblBoards(), "Id", "NameAr");
        //    var Event = _EventDomain.GetTblEventsById(id);
        //    if (Event == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(Event);

        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(EventViewModel Event)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (Event.EventStartDate <= Event.EventEndtDate) //&& Event.Time < Event.EndRegTime.TimeOfDay
        //            {
        //                Event.Id = Guid.NewGuid();
        //                int check = _EventDomain.AddEvent(Event);

        //                if (check == 1)
        //                {
        //                    ViewData["Successful"] = "تم تعديل الحدث بنجاح";

        //                }
        //                else
        //                {
        //                    ViewData["Falied"] = "حدث خطأ تعديل";
        //                }
        //            }
        //            else
        //            {
        //                if (Event.EventStartDate > Event.EventEndtDate)
        //                {
        //                    ViewData["Falied"] = "تأكد من تاريخ الحدث";
        //                }
        //                //else if (Event.Time >= Event.EndRegTime.TimeOfDay)
        //                //{
        //                //    ViewData["Falied"] = "تأكد من الساعة";
        //                //}
        //                _EventDomain.UpdateEvent(Event);
        //                return View(Event);

        //            }

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ViewData["Falied"] = "حدث خطأ في أثناء التعديل";
        //    }
        //    return View(Event);
        //}
        

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(Guid id)
        //{
        //    string Successful = "";
        //    string Falied = "";
        //    try
        //    {


        //        int check = _EventDomain.DeleteEvent(id);
        //        if (check == 1)
        //        {
        //            Successful = "تم حذف الحدث بنجاح";
        //        }

        //        else
        //        {
        //            Falied = "حدث خطأ";


        //        }
                

        //    }
        //    catch (Exception ex)
        //    {
        //        Falied = "حدث خطأ";

        //    }
        //    return RedirectToAction(nameof(Index), new { Successful = Successful, Falied = Falied });
        
        
        //}


//    }
//}

    
