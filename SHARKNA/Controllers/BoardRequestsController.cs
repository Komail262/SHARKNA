using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SHARKNA.Controllers
{
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

        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        public async Task<IActionResult> Index(string Successful = "", string Falied = "")
        {
            if (!string.IsNullOrEmpty(Successful))
                ViewData["Successful"] = Successful;
            else if (!string.IsNullOrEmpty(Falied))
                ViewData["Falied"] = Falied;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userBoardRequests = await _boardRequestsDomain.GetTblBoardRequestsByUserAsync(username);

            return View(userBoardRequests);
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> UserDetails()
        {
            var username = User.FindFirst(ClaimTypes.GivenName)?.Value;
            var userGender = User.FindFirst(ClaimTypes.Gender)?.Value ?? "NotSpecified";
            var userBoardRequests = await _BoardDomain.GetTblBoardsAsync(userGender);
            return View(userBoardRequests);
        }

        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        public async Task<IActionResult> Archive()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userBoardRequests = await _boardRequestsDomain.GetTblBoardRequestsByUserAsync(username);

            return View(userBoardRequests);
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public async Task<IActionResult> Create(Guid boardId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _UserDomain.GetUserFERAsync(username);
            var board = await _BoardDomain.GetTblBoardByIdAsync(boardId);

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

        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        public async Task<IActionResult> Details(Guid id)
        {
            var request = await _boardRequestsDomain.GetBoardRequestByIdAsync(id);
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

                bool canMakeNewRequest = await _boardRequestsDomain.CanUserMakeNewRequestAsync(user.Email, BoardReq.BoardId);

                if (!canMakeNewRequest)
                {
                    ViewData["Falied"] = "لا يمكنك تقديم طلب جديد لهذا النادي، لديك طلب سابق.";
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

                    int check = await _boardRequestsDomain.AddBoardReqAsync(BoardReq, UserName ,username);
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
        public async Task<IActionResult> CancelRequest(Guid id)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                await _boardRequestsDomain.CancelRequestAsync(id ,username);
                ViewData["Successful"] = "تم إلغاء الطلب بنجاح.";
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء محاولة إلغاء الطلب.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
