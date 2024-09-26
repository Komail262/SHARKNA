using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Domain;
using SHARKNA.Models; 
using SHARKNA.ViewModels;
using System;
using System.Globalization;
using System.Linq; 
using System.Security.Claims;
using System.Threading.Tasks;

namespace SHARKNA.Controllers
{
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

        //صفحة الطلبات للمستخدم
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            
            string username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Users");
            }

            var userRequests = await _eventRequestDomain.GetEventRequestsByUserAsync(username);

            return View(userRequests);
        }



        //get for user
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> Create()
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Users");
            }

     
            ViewBag.Username = username;

            // جلب الـ Boards المتعلقة بالمستخدم الحالي
            var userBoards = await _eventRequestDomain.GetTblBoardsByUserAsync(username);
            ViewBag.BoardsList = new SelectList(userBoards, "Id", "NameAr");

            // تمرير بيانات `tblBoardMembers` إلى الـ View باستخدام ViewBag
            var userBoardMembers = userBoards.SelectMany(b => b.BoardMembers).Where(bm => bm.UserName == username).ToList();
            ViewBag.UserBoardMembers = userBoardMembers;

            return View();
        }


        //post for user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            try
            {
                
                string username = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    // في حال عدم وجود المستخدم، أعِد توجيهه إلى صفحة تسجيل الدخول
                    return RedirectToAction("Login", "Users");
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


        //get for user
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> Edit(Guid id)
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

        //post for users
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
                    return RedirectToAction("Login", "Users");
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

                    ViewData["Successful"] = "تم تعديل الحدث بنجاح .";
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
                string username = User.FindFirst(ClaimTypes.Name)?.Value; 
                await _eventRequestDomain.CancelEventRequestAsync(id, username);
                ViewData["Successful"] = "تم إلغاء الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إلغاء الطلب.";
            }

           
            string currentUser = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToAction("Login", "Users");

            }

            var userEventRequests = await _eventRequestDomain.GetEventRequestsByUserAsync(currentUser);

            return View("Index", userEventRequests);
        }




        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
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

      

       



    }
}



