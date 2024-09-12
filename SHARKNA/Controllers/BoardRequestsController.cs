using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SHARKNA.Controllers
{
    public class BoardRequestsController : Controller
    {
        private readonly BoardRequestsDomain _boardRequestsDomain;
        private readonly BoardDomain _BoardDomain;
        private readonly RequestStatusDomain _RequestStatusDomain;
        private readonly BoardMembersDomain _BoardMembersDomain;
        private readonly UserDomain _UserDomain;

        public BoardRequestsController(BoardRequestsDomain boardRequestsDomain, BoardDomain BoardDomain, RequestStatusDomain requestStatusDomain, BoardMembersDomain boardMembersDomain , UserDomain userDomain)
        {
            _boardRequestsDomain = boardRequestsDomain;
            _BoardDomain = BoardDomain;
            _RequestStatusDomain = requestStatusDomain;
            _BoardMembersDomain = boardMembersDomain;
            _UserDomain = userDomain;   
        }

        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userBoardRequests = _boardRequestsDomain.GetTblBoardRequestsByUser(username);

            return View(userBoardRequests);
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
        public IActionResult Create(Guid boardId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; 
            var user = _UserDomain.GetUserFER(username);
            var board = _BoardDomain.GetTblBoards().FirstOrDefault(b => b.Id == boardId);

            var model = new BoardRequestsViewModel
            {
                UserName = username,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                FullNameAr = user.FullNameAr,
                BoardId = boardId,
                BoardName = board.NameAr,
                BoardDescription = board.DescriptionAr, 
                FullNameEn = user.FullNameEn
            };
                        
            return View(model); 
        }

        public IActionResult Details(Guid id)
        {
            var request = _boardRequestsDomain.GetBoardRequestById(id);
            return View(request);
        }



        //[Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BoardRequestsViewModel BoardReq ,string UserName)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = _UserDomain.GetUserFER(username);

                
                bool requestExists = _boardRequestsDomain.CheckRequestExists(user.Email, BoardReq.BoardId);
                if (requestExists)
                {
                    ViewData["Falied"] = "لقد قمت بالفعل بتقديم طلب لهذا النادي.";
                   
                    return View(BoardReq);
                }

               // ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr", BoardReq.BoardId);
                if (ModelState.IsValid)
                {
                    BoardReq.Id = Guid.NewGuid();
                    BoardReq.UserName = username;
                    BoardReq.Email = user.Email;
                    BoardReq.MobileNumber = user.MobileNumber;
                    BoardReq.FullNameAr = user.FullNameAr;
                    BoardReq.FullNameEn = user.FullNameEn;

                    int check = _boardRequestsDomain.AddBoardReq(BoardReq , UserName);
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


        //[Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelRequest(Guid id)
        {
            try
            {
                _boardRequestsDomain.CancelRequest(id);
                ViewData["Successful"] = "تم إلغاء الطلب بنجاح.";
            }
            catch (Exception ex)
            {
                ViewBag["Falied"] = "حدث خطأ أثناء محاولة إلغاء الطلب.";
            }

            return RedirectToAction(nameof(Index));
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

