using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Domain;
using SHARKNA.Models; // إضافة الـ Models للوصول إلى DbContext والـ Models
using SHARKNA.ViewModels;
using System;
using System.Globalization;
using System.Linq; // إضافة LINQ للوصول إلى العمليات مثل .Where()
using System.Security.Claims;
using System.Threading.Tasks;

namespace SHARKNA.Controllers
{
    public class EventRequestsController : Controller
    {
        private readonly EventRequestsDomain _eventRequestDomain;
        private readonly EventDomain _eventDomain;
        private readonly UserDomain _UserDomain;
        private readonly SHARKNAContext _context; // إضافة DbContext كحقل خاص

        public EventRequestsController(EventRequestsDomain eventRequestDomain, EventDomain eventDomain, UserDomain userDomain, SHARKNAContext context)
        {
            _eventRequestDomain = eventRequestDomain;
            _eventDomain = eventDomain;
            _UserDomain = userDomain;
            _context = context; // حقن DbContext من خلال الـ Constructor
        }

        public async Task<IActionResult> Index()
        {
            // احصل على اسم المستخدم الحالي من Claims
            string username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                // في حال عدم وجود المستخدم، أعِد توجيهه إلى صفحة تسجيل الدخول
                return RedirectToAction("Login", "Account");
            }

            // جلب الطلبات المتعلقة بالمستخدم الحالي فقط
            var userRequests = await _eventRequestDomain.GetEventRequestsByUserAsync(username);

            // تمرير الطلبات إلى الـ View
            return View(userRequests);
        }


        public async Task<IActionResult> Admin()
        {
            var eventRequests = await _eventRequestDomain.GetTblEventRequestsAsync();
            return View(eventRequests);
        }
        public async Task<IActionResult> AdminArsh()
        {
            var eventRequests = await _eventRequestDomain.GetTblEventRequestsAsync();
            return View(eventRequests);
        }



        public async Task<IActionResult> Create()
        {
            // احصل على اسم المستخدم الحالي
            string username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                // في حال عدم وجود المستخدم، أعِد توجيهه إلى صفحة تسجيل الدخول
                return RedirectToAction("Login", "Account");
            }

            // احفظ اسم المستخدم في ViewBag لعرضه في الـ View
            ViewBag.Username = username;

            // جلب الـ Boards المتعلقة بالمستخدم الحالي
            var userBoards = await _eventRequestDomain.GetTblBoardsByUserAsync(username);
            ViewBag.BoardsList = new SelectList(userBoards, "Id", "NameAr");

            // تمرير بيانات `tblBoardMembers` إلى الـ View باستخدام ViewBag
            var userBoardMembers = userBoards.SelectMany(b => b.BoardMembers).Where(bm => bm.UserName == username).ToList();
            ViewBag.UserBoardMembers = userBoardMembers;

            return View();
        }










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
                    return RedirectToAction("Login", "Account");
                }

                // احفظ اسم المستخدم في ViewBag لعرضه في الـ View
                ViewBag.Username = username;

                // جلب الـ Boards المتعلقة بالمستخدم الحالي
                var userBoards = await _eventRequestDomain.GetTblBoardsByUserAsync(username);
                ViewBag.BoardsList = new SelectList(userBoards, "Id", "NameAr", eventViewModel.BoardId);

                // جلب جميع السجلات من جدول tblBoardMembers المتعلقة بالمستخدم الحالي، مع جلب اسم البورد
                var userBoardMembers = await _context.tblBoardMembers
                    .Include(bm => bm.Board) // استخدام Include لجلب بيانات Board
                    .Where(bm => bm.UserName == username)
                    .ToListAsync();

                // تمرير بيانات `tblBoardMembers` إلى الـ View باستخدام ViewBag
                ViewBag.UserBoardMembers = userBoardMembers;

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
                        RequestStatusId = Guid.Parse("93D729FA-E7FA-4EA6-BB16-038454F8C5C2"),
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


        public async Task<IActionResult> Edit(Guid id)
        {
            // احصل على اسم المستخدم الحالي
            string username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                // في حال عدم وجود المستخدم، أعِد توجيهه إلى صفحة تسجيل الدخول
                return RedirectToAction("Login", "Account");
            }

            // احفظ اسم المستخدم في ViewBag لعرضه في الـ View
            ViewBag.Username = username;

            // جلب الـ Boards المتعلقة بالمستخدم الحالي
            var userBoards = await _eventRequestDomain.GetTblBoardsByUserAsync(username);
            ViewBag.BoardsList = new SelectList(userBoards, "Id", "NameAr");

            // جلب جميع السجلات من جدول tblBoardMembers المتعلقة بالمستخدم الحالي، مع جلب اسم البورد
            var userBoardMembers = await _context.tblBoardMembers
                .Include(bm => bm.Board) // استخدام Include لجلب بيانات Board
                .Where(bm => bm.UserName == username)
                .ToListAsync();

            // تمرير بيانات `tblBoardMembers` إلى الـ View باستخدام ViewBag
            ViewBag.UserBoardMembers = userBoardMembers;
            var eventViewModel = await _eventDomain.GetTblEventsByIdAsync(id);
            if (eventViewModel == null)
            {
                return NotFound();
            }
            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel eventViewModel)
        {
            try
            {
                // احصل على اسم المستخدم الحالي
                string username = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    // في حال عدم وجود المستخدم، أعِد توجيهه إلى صفحة تسجيل الدخول
                    return RedirectToAction("Login", "Account");
                }

                // احفظ اسم المستخدم في ViewBag لعرضه في الـ View
                ViewBag.Username = username;

                // جلب الـ Boards المتعلقة بالمستخدم الحالي
                var userBoards = await _eventRequestDomain.GetTblBoardsByUserAsync(username);
                ViewBag.BoardsList = new SelectList(userBoards, "Id", "NameAr", eventViewModel.BoardId);

                // تمرير بيانات `tblBoardMembers` إلى الـ View باستخدام ViewBag
                var userBoardMembers = userBoards.SelectMany(b => b.BoardMembers).Where(bm => bm.UserName == username).ToList();
                ViewBag.UserBoardMembers = userBoardMembers;

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

                // تعديل الحدث في جدول tblEvents
                int check = await _eventDomain.UpdateEventAsync(eventViewModel, username);

                if (check == 1)
                {
                    // جلب طلب الحدث من جدول tblEventRequests باستخدام EventId
                    var eventRequest = await _context.tblEventRequests.FirstOrDefaultAsync(er => er.EventId == eventViewModel.Id);
                    if (eventRequest != null)
                    {
                        // تحديث BoardId في طلب الحدث
                        eventRequest.BoardId = eventViewModel.BoardId;
                        _context.Update(eventRequest);
                        await _context.SaveChangesAsync();
                    }

                    ViewData["Successful"] = "تم تعديل الحدث بنجاح وتم تحديث جميع السجلات ذات الصلة.";
                }
                else
                {
                    ViewData["Falied"] = "حدث خطأ أثناء التعديل.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = $"حدث خطأ أثناء التعديل: {ex.Message}";
            }

            return View(eventViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                await _eventRequestDomain.CancelEventRequestAsync(id, username);
                ViewData["Successful"] = "تم إلغاء الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إلغاء الطلب.";
            }

            // احصل على اسم المستخدم الحالي
            string currentUser = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(currentUser))
            {
                // في حال عدم وجود المستخدم، أعِد توجيهه إلى صفحة تسجيل الدخول
                return RedirectToAction("Login", "Account");
            }

            // جلب طلبات الفعاليات المتعلقة بالمستخدم الحالي، بما في ذلك الطلبات الملغاة
            var userEventRequests = await _eventRequestDomain.GetEventRequestsByUserAsync(currentUser);

            // قم بإعادة عرض صفحة الطلبات الخاصة بالمستخدم فقط
            return View("Index", userEventRequests);
        }






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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateRequestStatus(Guid id, Guid RequestStatusId, string RejectionReasons)
        //{
        //    try
        //    {
        //        string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
        //        if (username != null)
        //        {
        //            int result = await _eventRequestDomain.UpdateRequestStatusAsync(id, RequestStatusId, RejectionReasons, username);
        //            if (result == 1)
        //            {
        //                return RedirectToAction(nameof(Index), new { Successful = "تم تحديث حالة الطلب بنجاح." });
        //            }
        //            else
        //            {
        //                return RedirectToAction(nameof(Index), new { Failed = "فشل في تحديث حالة الطلب." });
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction(nameof(Index), new { Failed = "فشل في الحصول على اسم المستخدم." });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction(nameof(Index), new { Failed = "حدث خطأ أثناء تحديث حالة الطلب." });
        //    }
        //}
    }
}



