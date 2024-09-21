using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SHARKNA.Controllers
{
    public class BoardTalRequestsController : Controller
    {
        private readonly BoardTalRequestsDomain _BoardTalRequestsDomain;
        private readonly RequestStatusDomain _RequestStatusDomain;
        private readonly BoardDomain _BoardDomain;
        private readonly UserDomain _UserDomain;


        public BoardTalRequestsController(BoardTalRequestsDomain BoardTalRequestsDomain, RequestStatusDomain RequestStatusDomain, BoardDomain BoardDomain, UserDomain userDomain)
        {
            _BoardTalRequestsDomain = BoardTalRequestsDomain;
            _RequestStatusDomain = RequestStatusDomain;
            _BoardDomain = BoardDomain;
            _UserDomain = userDomain;

        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (!string.IsNullOrEmpty(Successful))
                ViewData["Successful"] = Successful;
            else if (!string.IsNullOrEmpty(Falied))
                ViewData["Falied"] = Falied;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userTalBoardRequests = _BoardTalRequestsDomain.GetTblBoardTalRequestsByUser(username);
            return View(userTalBoardRequests);
        }

        //public IActionResult AllRequests()
        //{
        //    var Btal = _BoardTalRequestsDomain.GetTblBoardTalRequests();
        //    return View(Btal);
        //}

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        public IActionResult UserDetails(Guid id)
        {
            var userTalBoardRequests = _BoardDomain.GetTblBoards();

            return View(userTalBoardRequests);
        }
        public IActionResult Archive()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userTalBoardRequests = _BoardTalRequestsDomain.GetTblBoardTalRequestsByUser(username);

            return View(userTalBoardRequests);
        }



        public async Task<IActionResult> Create(Guid boardId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _UserDomain.GetUserFERAsync(username);
            var board = _BoardDomain.GetTblBoards().FirstOrDefault(b => b.Id == boardId);


            var model = new BoardTalRequestsViewModel
            {

                UserName = username,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                FullNameAr = user.FullNameAr,
                BoardId = boardId,
                BoardName = board.NameAr,
                BoardDescription = board.DescriptionAr,
                FullNameEn = user.FullNameEn,



            };

            return View(model);
        }

        public IActionResult RequestDetails(Guid id)
        {
            var request = _BoardTalRequestsDomain.GetBoardTalRequestsById(id);
            return View(request);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardTalRequestsViewModel BordTalReq, string UserName)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _UserDomain.GetUserFERAsync(username);



                bool requestExists = _BoardTalRequestsDomain.CheckRequestExists(BordTalReq.Email, BordTalReq.BoardId);
                if (requestExists)
                {

                    ViewData["Falied"] = "لقد قمت بالفعل بتقديم طلب لهذا النادي.";
                    return View(BordTalReq);
                }

                if (ModelState.IsValid)
                {
                    BordTalReq.Id = Guid.NewGuid();
                    BordTalReq.UserName = username;
                    BordTalReq.Email = user.Email;
                    BordTalReq.MobileNumber = user.MobileNumber;
                    BordTalReq.FullNameAr = user.FullNameAr;
                    BordTalReq.FullNameEn = user.FullNameEn;

                    var skills = BordTalReq.Skills;
                    var experiences = BordTalReq.Experiences;



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


        [Authorize(Roles = "NoRole,User,Admin,SuperAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelRequest(Guid id)
        {
            try
            {
                _BoardTalRequestsDomain.CancelRequest(id);
                TempData["Message"] = "تم إلغاء الطلب بنجاح."; // رسالة نجاح
            }
            catch (Exception)
            {
                TempData["Error"] = "حدث خطأ أثناء محاولة إلغاء الطلب."; // رسالة خطأ
            }

            return RedirectToAction(nameof(Index));
        }



    }
}