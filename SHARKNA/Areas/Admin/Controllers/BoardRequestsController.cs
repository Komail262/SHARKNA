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
    public class BoardRequestsController : Controller
    {
            private readonly BoardRequestsDomain _boardRequestsDomain;
            private readonly BoardDomain _BoardDomain;
            private readonly RequestStatusDomain _RequestStatusDomain;
            private readonly BoardMembersDomain _BoardMembersDomain;
            private readonly UserDomain _UserDomain;

            public BoardRequestsController(BoardRequestsDomain boardRequestsDomain, BoardDomain BoardDomain, RequestStatusDomain requestStatusDomain, BoardMembersDomain boardMembersDomain, UserDomain userDomain)
            {
                _boardRequestsDomain = boardRequestsDomain;
                _BoardDomain = BoardDomain;
                _RequestStatusDomain = requestStatusDomain;
                _BoardMembersDomain = boardMembersDomain;
                _UserDomain = userDomain;
            }

            [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
            public IActionResult UserDetails()
            {
                var userBoardRequests = _BoardDomain.GetTblBoards();
                return View(userBoardRequests);
            }

            [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
            public IActionResult Admin()
            {
                var BoardReq = _boardRequestsDomain.GetTblBoardRequests();
                return View(BoardReq);
            }

            [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
            public IActionResult Archive()
            {
                var BoardReq = _boardRequestsDomain.GetTblBoardRequests();
                return View(BoardReq);
            }

            [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
            public async Task<IActionResult> Create(Guid boardId)
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _UserDomain.GetUserFERAsync(username);
                var board = _BoardDomain.GetTblBoards().FirstOrDefault(b => b.Id == boardId);

                var model = new BoardRequestsViewModel
                {
                    //UserName = username,
                    //Email = user.Email,
                    //MobileNumber = user.MobileNumber,
                    //FullNameAr = user.FullNameAr,
                    //FullNameEn = user.FullNameEn,
                    BoardId = boardId,
                    BoardName = board.NameAr,
                    BoardDescription = board.DescriptionAr,
                    
                };

                return View(model);

            }

            public IActionResult Details(Guid id)
            {
                var request = _boardRequestsDomain.GetBoardRequestById(id);
                return View(request);
            }



            [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardRequestsViewModel BoardReq, string UserName)
        {
            try
            {
                var user = await _boardRequestsDomain.GetTblUsersByUserName(BoardReq.UserName); // Use the passed UserName instead of claim.
                if (user == null)
                {
                    ViewData["Falied"] = "لم يتم العثور على المستخدم";
                    return View(BoardReq);
                }

                bool requestExists = _boardRequestsDomain.CheckRequestExists(user.Email, BoardReq.BoardId);
                if (requestExists)
                {
                    ViewData["Falied"] = "لقد قمت بالفعل بتقديم طلب لهذا النادي.";
                    return View(BoardReq);
                }

                if (ModelState.IsValid)
                {
                    BoardReq.Id = Guid.NewGuid();

                    int check = _boardRequestsDomain.AddBoardReq(BoardReq, UserName);
                    if (check == 1)
                    {
                        ViewData["Successful"] = "تم تسجيل طلبك بنجاح";
                    }
                    else
                    {
                        ViewData["Falied"] = "حدث خطأ";
                    }
                    return View(BoardReq);
                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ";
            }

            return View(BoardReq);
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
            public async Task<IActionResult> Accept(Guid id)
            {
                try
                {
                    await _boardRequestsDomain.Accept(id);
                    ViewData["Successful"] = "تم قبول الطلب بنجاح.";
                }
                catch (Exception)
                {
                    ViewBag.Falied = "حدث خطأ أثناء محاولة إلغاء الطلب.";

                }

                return RedirectToAction(nameof(Admin));
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Reject(Guid id, string rejectionReason)
            {
                try
                {
                    _boardRequestsDomain.Reject(id, rejectionReason);
                    ViewData["Successful"] = "تم رفض الطلب بنجاح.";
                }
                catch (Exception)
                {
                    ViewBag["Falied"] = "حدث خطأ أثناء محاولة رفض الطلب.";
                }

                return RedirectToAction(nameof(Admin));
            }

        }
    }

