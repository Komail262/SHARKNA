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
        public IActionResult Index()
        {
            var BoardReq = _boardRequestsDomain.GetTblBoardRequests();
            return View(BoardReq);
        }

        public IActionResult Admin()
        {
            var BoardReq = _boardRequestsDomain.GetTblBoardRequests();
            return View(BoardReq);
        }
        public IActionResult Create()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; 
            var user = _UserDomain.GetUserFER(username);

            var model = new BoardRequestsViewModel
            {
                UserName = username,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                FullNameAr = user.FullNameAr,
                FullNameEn = user.FullNameEn
            };

            ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr");

            return View(model); 
        }

        public IActionResult Details(Guid id)
        {
            var request = _boardRequestsDomain.GetBoardRequestById(id);
            return View(request);
        }




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
                    ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr", BoardReq.BoardId);
                    return View(BoardReq);
                }

                ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr", BoardReq.BoardId);
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelRequest(Guid id)
        {
            try
            {
                _boardRequestsDomain.CancelRequest(id);
                ViewData["Successful"] = "تم إلغاء الطلب بنجاح.";
            }
            catch (Exception)
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

