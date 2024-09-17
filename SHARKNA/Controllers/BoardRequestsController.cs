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
        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (!string.IsNullOrEmpty(Successful))
                ViewData["Successful"] = Successful;
            else if (!string.IsNullOrEmpty(Falied))
                ViewData["Falied"] = Falied;

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

        public IActionResult Archive()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userBoardRequests = _boardRequestsDomain.GetTblBoardRequestsByUser(username);

            return View(userBoardRequests);
        }
        
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> Create(Guid boardId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _UserDomain.GetUserFERAsync(username);
            var board =  _BoardDomain.GetTblBoards().FirstOrDefault(b => b.Id == boardId);

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



        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardRequestsViewModel BoardReq, string UserName)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _UserDomain.GetUserFERAsync(username);


                bool requestExists =  _boardRequestsDomain.CheckRequestExists(user.Email, BoardReq.BoardId);
                if (requestExists)
                {
                    ViewData["Falied"] = "لقد قمت بالفعل بتقديم طلب لهذا النادي.";

                    return View(BoardReq);
                }

                if (ModelState.IsValid)
                {
                    BoardReq.Id = Guid.NewGuid();
                    BoardReq.UserName = username;
                    BoardReq.Email = user.Email;
                    BoardReq.MobileNumber = user.MobileNumber;
                    BoardReq.FullNameAr = user.FullNameAr;
                    BoardReq.FullNameEn = user.FullNameEn;

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


        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
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


    }




}
         

