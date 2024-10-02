using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Super Admin,Editor")]

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

        
        public async Task<IActionResult> UserDetails()
        {
            var userBoardRequests =  _BoardDomain.GetTblBoards();
            return View(userBoardRequests);
        }


        public async Task<IActionResult> Admin()
        {
            var BoardReq = await _boardRequestsDomain.GetTblBoardRequestsAsync();
            return View(BoardReq);
        }

        public async Task<IActionResult> Archive()
        {
            var BoardReq = await _boardRequestsDomain.GetTblBoardRequestsAsync();
            return View(BoardReq);
        }

        public async Task<IActionResult> Create(Guid boardId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _UserDomain.GetUserFERAsync(username);
            var board = await _BoardDomain.GetTblBoardByIdAsync(boardId);

            var model = new BoardRequestsViewModel
            {
                
                BoardId = boardId,
                BoardName = board.NameAr,
                BoardDescription = board.DescriptionAr,
            };

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var request = await _boardRequestsDomain.GetBoardRequestByIdAsync(id);
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardRequestsViewModel BoardReq, string UserName)
        {
            try
            {
                //var user = await _boardRequestsDomain.GetAllUsers(BoardReq.UserName); 
                var user = await _boardRequestsDomain.GetAllUsers(); 
                if (user == null)
                {
                    ViewData["Falied"] = "لم يتم العثور على المستخدم";
                    return View(BoardReq);
                }

                if (ModelState.IsValid)
                {
                    string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                    BoardReq.Id = Guid.NewGuid();
                    await _boardRequestsDomain.AddBoardReqAdminAsync(BoardReq);
                    ViewData["Successful"] = "تم التسجيل بنجاح ";
                    int check = await _boardRequestsDomain.AddBoardReqAsync(BoardReq, UserName , username);
                    if (check == 1)
                    {
                        ViewData["Successful"] = "تم التسجيل بنجاح";
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

        public async Task<UserViewModel> GetUserInfo(string id)
        {
            return await _UserDomain.GetTblUserByUserName(id);
        }


        [HttpGet]
        public async Task<IActionResult> Accept(Guid id)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                await _boardRequestsDomain.AcceptAsync(id , username);
                ViewData["Successful"] = "تم قبول الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء محاولة إلغاء الطلب.";
            }

            return RedirectToAction(nameof(Admin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id, string rejectionReason)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                await _boardRequestsDomain.RejectAsync(id, rejectionReason, username);
                ViewData["Successful"] = "تم رفض الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewData["Falied"] = "حدث خطأ أثناء محاولة رفض الطلب.";
            }

            return RedirectToAction(nameof(Admin));
        }
    }
}
