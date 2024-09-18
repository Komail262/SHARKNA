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


        //post for user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            try
            {
                // احصل على اسم المستخدم الحالي
                string username = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    // في حال عدم وجود المستخدم، أعِد توجيهه إلى صفحة تسجيل الدخول
                    return RedirectToAction("Login", "Users");
                }

                // احفظ اسم المستخدم في ViewBag لعرضه في الـ View
                ViewBag.Username = username;


                // التحقق من صحة النموذج
                if (!ModelState.IsValid)
                {
                    return View(eventViewModel);
                }

                // تحقق من قيمة MaxAttendence
                if (eventViewModel.MaxAttendence <= 0)
                {
                    ViewData["Falied"] = "الحد الأقصى للحضور يجب أن يكون قيمة موجبة.";
                    return View(eventViewModel);
                }

                // تحقق من تواريخ البداية والنهاية
                if (eventViewModel.EventStartDate >= eventViewModel.EventEndtDate)
                {
                    ViewData["Falied"] = "تاريخ البداية يجب أن يكون أصغر من تاريخ النهاية.";
                    return View(eventViewModel);
                }

                if (eventViewModel.EventStartDate < DateTime.Now)
                {
                    ViewData["Falied"] = "تاريخ البداية يجب أن يكون في المستقبل.";
                    return View(eventViewModel);
                }

                // إنشاء معرف جديد للحدث
                eventViewModel.Id = Guid.NewGuid();

                // أضف الحدث
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
                    ViewData["Successful"] = "تم تقديم الطلب بنجاح وهو الآن قيد المراجعة من قبل المشرف";
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

            // في حال وجود خطأ، يتم إعادة عرض النموذج مع البيانات المحملة
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
                return RedirectToAction(nameof(Index), new { Successful = "تم قبول الطلب بنجاح." });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { Falied = "حدث خطأ أثناء قبول الطلب." });
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
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims


                await _eventRequestDomain.RejectRequestAsync(id, rejectionReason, username);
                return RedirectToAction(nameof(Index), new { Successful = "تم رفض الطلب بنجاح." });

            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { Failed = "حدث خطأ أثناء رفض الطلب." });
            }
        }



    }
}



