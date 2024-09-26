using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System.Security;
using System.Security.Claims;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BoardTalRequestsController : Controller
    {
        private readonly BoardTalRequestsDomain _BoardTalRequestsDomain;
        private readonly BoardDomain _BoardDomain;
        private readonly RequestStatusDomain _RequestStatusDomain;
        private readonly UserDomain _UserDomain;

        public BoardTalRequestsController(BoardTalRequestsDomain BoardTalRequestsDomain, BoardDomain BoardDomain, RequestStatusDomain requestStatusDomain, UserDomain userDomain)
        {
            _BoardTalRequestsDomain = BoardTalRequestsDomain;
            _BoardDomain = BoardDomain;
            _RequestStatusDomain = requestStatusDomain;
            _UserDomain = userDomain;
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public IActionResult UserDetails()
        {
            var userTalBoardRequests = _BoardDomain.GetTblBoards();
            return View(userTalBoardRequests);
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public IActionResult AllRequests()
        {
            var BordTalReq = _BoardTalRequestsDomain.GetTblBoardTalRequests();
            return View(BordTalReq);
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public IActionResult Archive()
        {
            var BordTalReq = _BoardTalRequestsDomain.GetTblBoardTalRequests();
            return View(BordTalReq);
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> Create(Guid boardId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _UserDomain.GetUserFERAsync(username);
            var board = _BoardDomain.GetTblBoards().FirstOrDefault(b => b.Id == boardId);

            var model = new BoardTalRequestsViewModel
            {

                BoardId = boardId,
                BoardName = board.NameAr,
                BoardDescription = board.DescriptionAr,

            };

            return View(model);

        }

        public IActionResult RequestDetails(Guid id)
        {
            var request = _BoardTalRequestsDomain.GetBoardTalRequestsById(id);
            return View(request);
        }



        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardTalRequestsViewModel BordTalReq, string UserName)
        {
            try
            {
                var user = await _BoardTalRequestsDomain.GetTblUsersByUserName(BordTalReq.UserName); // Use the passed UserName instead of claim.
                if (user == null)
                {
                    ViewData["Falied"] = "لم يتم العثور على المستخدم";
                    return View(BordTalReq);
                }

                bool requestExists = _BoardTalRequestsDomain.CheckRequestExists(BordTalReq.Email, BordTalReq.BoardId);
                if (requestExists)
                {
                    ViewData["Falied"] = "لقد قمت بالفعل بتقديم طلب لهذا النادي.";
                    return View(BordTalReq);
                }

                if (ModelState.IsValid)
                {
                    BordTalReq.Id = Guid.NewGuid();

                    int check = _BoardTalRequestsDomain.AddUser(BordTalReq, UserName);
                    if (check == 1)
                    {
                        ViewData["Successful"] = "تم تسجيل طلبك بنجاح";
                    }
                    else
                    {
                        ViewData["Falied"] = "حدث خطأ";
                    }
                    return View(BordTalReq);
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ";
            }

            return View(BordTalReq);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            var user = await _UserDomain.GetTblUserByUserName(id); // Fetch user by username
            if (user != null)
            {
                // Return the user details as JSON
                return Json(new { fullNameAr = user.FullNameAr, fullNameEn = user.FullNameEn, email = user.Email, mobileNumber = user.MobileNumber });
            }
            else
            {
                return Json(null); // Return null if user not found
            }
        }





        [HttpGet]
        public async Task<IActionResult> Accepted(Guid id)
        {
            try
            {
                await _BoardTalRequestsDomain.Accepted(id);
                ViewData["Successful"] = "تم قبول الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewBag.Falied = "حدث خطأ أثناء محاولة إلغاء الطلب.";

            }

            return RedirectToAction(nameof(AllRequests));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rejected(Guid id, string rejectionReason)
        {
            try
            {
                _BoardTalRequestsDomain.Rejected(id, rejectionReason);
                ViewData["Successful"] = "تم رفض الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewBag["Falied"] = "حدث خطأ أثناء محاولة رفض الطلب.";
            }

            return RedirectToAction(nameof(AllRequests));
        }

    }
}