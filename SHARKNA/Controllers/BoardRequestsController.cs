using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;

namespace SHARKNA.Controllers
{
    public class BoardRequestsController : Controller
    {
        private readonly BoardRequestsDomain _boardRequestsDomain;
        private readonly BoardDomain _BoardDomain;
        private readonly RequestStatusDomain _RequestStatusDomain;
        private readonly BoardMembersDomain _BoardMembersDomain;

        public BoardRequestsController(BoardRequestsDomain boardRequestsDomain, BoardDomain BoardDomain, RequestStatusDomain requestStatusDomain, BoardMembersDomain boardMembersDomain)
        {
            _boardRequestsDomain = boardRequestsDomain;
            _BoardDomain = BoardDomain;
            _RequestStatusDomain = requestStatusDomain;
            _BoardMembersDomain = boardMembersDomain;
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

            ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr");
            return View();
        }

        public IActionResult Details(Guid id)
        {
            var request = _boardRequestsDomain.GetBoardRequestById(id);
            return View(request);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(BoardRequestsViewModel BoardReq)
        {
            try
            {
                ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr", BoardReq.BoardId);
                if (ModelState.IsValid)
                {
                    if (_boardRequestsDomain.IsEmailDuplicate(BoardReq.Email))
                    {
                        ViewData["Falied"] = "البريد الإلكتروني مستخدم بالفعل";
                        return View(BoardReq);
                    }

                    BoardReq.Id = Guid.NewGuid();

                    int check = _boardRequestsDomain.AddBoardReq(BoardReq);
                    if (check == 1)
                        ViewData["Successful"] = "تم تسجيل طلبك بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ";
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
                ViewBag["Falied"] = "حدث خطأ أثناء محاولة إلغاء الطلب.";
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
                ViewData["Successful"] = "تم قبول الطلب بنجاح.";
            }
            catch (Exception)
            {
                ViewBag["Falied"] = "حدث خطأ أثناء محاولة إلغاء الطلب.";
            }

            return RedirectToAction(nameof(Admin));
        }
    }




}

