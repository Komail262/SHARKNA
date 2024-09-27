using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Security.Claims;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventRequestsController : Controller
    {
        private readonly EventRequestsDomain _eventRequestDomain;
        private readonly EventDomain _eventDomain;
        private readonly UserDomain _UserDomain;
        private readonly SHARKNAContext _context;

        public EventRequestsController(EventRequestsDomain eventRequestDomain, EventDomain eventDomain, UserDomain userDomain, SHARKNAContext context)
        {
            _eventRequestDomain = eventRequestDomain;
            _eventDomain = eventDomain;
            _UserDomain = userDomain;
            _context = context;
        }

        //صفحة طلبات تحت الدراسة للادمن
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> Admin()
        {
            var eventRequests = await _eventRequestDomain.GetTblEventRequestsAsync();
            return View(eventRequests);
        }

        //صفحة الارشيف للادمن
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> AdminArsh()
        {
            var eventRequests = await _eventRequestDomain.GetTblEventRequestsAsync();
            return View(eventRequests);
        }

        //Get for Admin
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]

        public async Task<IActionResult> CreateAdmin()
        {
            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAdminAsync(), "Id", "NameAr");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(EventViewModel eventViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                if (eventViewModel.MaxAttendence <= 0)
                {
                    ViewData["Falied"] = "الحد الأقصى للحضور يجب أن يكون قيمة موجبة.";
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                if (eventViewModel.EventStartDate >= eventViewModel.EventEndtDate)
                {
                    ViewData["Falied"] = "تاريخ البداية يجب أن يكون أصغر من أو يساوي تاريخ النهاية.";
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                if (eventViewModel.EventEndtDate < DateTime.Now)
                {
                    ViewData["Falied"] = "تاريخ النهاية يجب أن يكون في المستقبل.";
                    ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr", eventViewModel.BoardId);
                    return View(eventViewModel);
                }

                eventViewModel.Id = Guid.NewGuid();
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                int check = await _eventDomain.AddEventAsync(eventViewModel, username);

                if (check == 1)
                {
                    var eventRequestViewModel = new EventRequestViewModel
                    {
                        Id = Guid.NewGuid(),
                        EventId = eventViewModel.Id,
                        BoardId = eventViewModel.BoardId,
                        RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152"),
                        RejectionReasons = null
                    };

                    await _eventRequestDomain.AddEventViewRequestAsync(eventRequestViewModel, username);
                    ViewData["Successful"] = "تم إضافة الحدث والطلب بنجاح";
                }
                else
                {
                    ViewData["Falied"] = "حدث خطأ أثناء الإضافة.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = $"حدث خطأ: {ex.Message}";
            }

            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
            return View(eventViewModel);
        }


                


        // عرض الفعاليات النشطة
        public async Task<IActionResult> ActiveEvents()
        {
            var activeEvents = await (from eventRequest in _context.tblEventRequests
                                      join eventItem in _context.tblEvents on eventRequest.EventId equals eventItem.Id
                                      where eventItem.IsActive && !eventItem.IsDeleted &&
                                            eventRequest.RequestStatusId == Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152")
                                      select new EventRequestViewModel
                                      {
                                          Id = eventRequest.Id,
                                          EventId = eventItem.Id,
                                          EventName = eventItem.EventTitleAr,
                                          EventStartDate = eventItem.EventStartDate,
                                          EventEndtDate = eventItem.EventEndtDate,
                                          RequestStatusName = eventRequest.RequestStatus.RequestStatusAr,
                                          BoardName = eventRequest.Board.NameAr
                                      })
                                      .ToListAsync();

            return View(activeEvents);
        }

        // أرشفة الفعالية
        [HttpPost]
        public async Task<IActionResult> ArchiveEvent(Guid eventId)
        {
            try
            {
                var eventToArchive = await _context.tblEvents.FindAsync(eventId);
                if (eventToArchive != null)
                {
                    eventToArchive.IsActive = false; // تحويل الفعالية إلى الأرشيف
                    await _context.SaveChangesAsync();
                    ViewData["Successful"] = "تم أرشفة الفعالية بنجاح.";
                }
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء أرشفة الفعالية.";
            }

            return RedirectToAction("ActiveEvents");
        }

        // عرض الفعاليات المؤرشفة
        public async Task<IActionResult> ArchivedEvents()
        {
            var archivedEvents = await (from eventRequest in _context.tblEventRequests
                                        join eventItem in _context.tblEvents on eventRequest.EventId equals eventItem.Id
                                        where !eventItem.IsActive && !eventItem.IsDeleted
                                        select new EventRequestViewModel
                                        {
                                            Id = eventRequest.Id,
                                            EventId = eventItem.Id,
                                            EventName = eventItem.EventTitleAr,
                                            EventStartDate = eventItem.EventStartDate,
                                            EventEndtDate = eventItem.EventEndtDate,
                                            RequestStatusName = eventRequest.RequestStatus.RequestStatusAr,
                                            BoardName = eventRequest.Board.NameAr
                                        })
                                        .ToListAsync();

            return View(archivedEvents);
        }

        // إلغاء أرشفة الفعالية
        [HttpPost]
        public async Task<IActionResult> UnarchiveEvent(Guid eventId)
        {
            try
            {
                var eventToUnarchive = await _context.tblEvents.FindAsync(eventId);
                if (eventToUnarchive != null)
                {
                    eventToUnarchive.IsActive = true; // إلغاء الأرشفة
                    await _context.SaveChangesAsync();
                    ViewData["Successful"] = "تم إلغاء الأرشفة بنجاح.";
                }
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إلغاء الأرشفة.";
            }

            return RedirectToAction("ArchivedEvents");
        }

        // حذف الفعالية
        [HttpPost]
        public async Task<IActionResult> DeleteEvent(Guid eventId)
        {
            try
            {
                var eventToDelete = await _context.tblEvents.FindAsync(eventId);
                if (eventToDelete != null)
                {
                    eventToDelete.IsDeleted = true; // تعيين الحالة كـ IsDeleted
                    await _context.SaveChangesAsync();
                    ViewData["Successful"] = "تم حذف الفعالية بنجاح.";
                }
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء حذف الفعالية.";
            }

            return RedirectToAction("ArchivedEvents");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
            var eventViewModel = await _eventDomain.GetTblEventsByIdAsync(id);
            if (eventViewModel == null)
            {
                return NotFound();
            }
            return View(eventViewModel);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var eventRequest = await _eventRequestDomain.GetEventRequestByIdAsync(id);
            if (eventRequest == null)
            {
                return NotFound();
            }
            ViewBag.RequestStatusList = new SelectList(await _eventRequestDomain.GetTblRequestStatusAsync(), "Id", "RequestStatusAr", eventRequest.RequestStatusId);
            return View(eventRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel eventViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (eventViewModel.MaxAttendence <= 0)
                    {
                        ViewData["Falied"] = "الحد الأقصى للحضور يجب أن يكون قيمة موجبة.";
                        ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
                        return View(eventViewModel);
                    }

                    if (eventViewModel.EventStartDate >= eventViewModel.EventEndtDate)
                    {
                        ViewData["Falied"] = "تاريخ البداية يجب أن يكون أصغر من أو يساوي تاريخ النهاية.";
                        ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
                        return View(eventViewModel);
                    }

                    if (eventViewModel.EventEndtDate < DateTime.Now)
                    {
                        ViewData["Falied"] = "تاريخ النهاية يجب أن يكون في المستقبل.";
                        ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
                        return View(eventViewModel);
                    }

                    string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                    int check = await _eventDomain.UpdateEventAsync(eventViewModel, username);

                    if (check == 1)
                    {
                        var eventRequest = await _context.tblEventRequests
                            .FirstOrDefaultAsync(er => er.EventId == eventViewModel.Id);

                        if (eventRequest != null)
                        {
                            // Update BoardId in the event request
                            eventRequest.BoardId = eventViewModel.BoardId;
                            _context.Update(eventRequest);
                            await _context.SaveChangesAsync();
                        }

                        ViewData["Successful"] = "تم تعديل الحدث بنجاح.";
                    }
                    else
                    {
                        ViewData["Falied"] = "حدث خطأ أثناء التعديل.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء التعديل.";
            }

            ViewBag.BoardsList = new SelectList(await _eventRequestDomain.GetTblBoardsAsync(), "Id", "NameAr");
            return View(eventViewModel);
        }





        //post For Admin
        [HttpGet]
        public async Task<IActionResult> Accept(Guid id)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                await _eventRequestDomain.AcceptRequestAsync(id, username);
                return RedirectToAction(nameof(Admin), new { Successful = "تم قبول الطلب بنجاح." });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Admin), new { Falied = "حدث خطأ أثناء قبول الطلب." });
            }
        }



        //Get for Admin
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> DetailsAdmin(Guid id)
        {
            var eventRequest = await _eventRequestDomain.GetEventRequestByIdAsync(id);
            if (eventRequest == null)
            {
                return NotFound();
            }
            ViewBag.RequestStatusList = new SelectList(await _eventRequestDomain.GetTblRequestStatusAsync(), "Id", "RequestStatusAr", eventRequest.RequestStatusId);
            return View(eventRequest);
        }

        //Post For Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id, string rejectionReason)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value;
                await _eventRequestDomain.RejectRequestAsync(id, rejectionReason, username);
                ViewData["Successful"] = "تم رفض الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء رفض الطلب.";
            }

            return RedirectToAction("Admin");
        }




    }
}



