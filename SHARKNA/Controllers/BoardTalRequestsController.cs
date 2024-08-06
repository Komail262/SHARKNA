using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SHARKNA.Controllers
{
  public class BoardTalRequestsController : Controller
    {
        private readonly BoardTalRequestsDomain _BoardTalRequestsDomain;
        private readonly RequestStatusDomain _RequestStatusDomain;
        private readonly BoardDomain _BoardDomain;


        public BoardTalRequestsController(BoardTalRequestsDomain BoardTalRequestsDomain, RequestStatusDomain RequestStatusDomain, BoardDomain BoardDomain)
        {
            _BoardTalRequestsDomain = BoardTalRequestsDomain;
            _RequestStatusDomain = RequestStatusDomain;
            _BoardDomain = BoardDomain;

        }
        
        public IActionResult Index()
        {
            var users = _BoardTalRequestsDomain.GetTblBoardTalRequests();
            return View(users);
        }


        public IActionResult Create()
        {
            ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr");
            //    ViewBag.RequestStatusOfList = new SelectList(_RequestStatusDomain.GetTblRequestStatus(), "Id", "NameAr");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BoardTalRequestsViewModel user)
        {
            try
            {
                ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr", user.BoardId);
                if (ModelState.IsValid)
                {
                    if (_BoardTalRequestsDomain.IsEmailDuplicate(user.Email))
                    {
                        ViewData["Falied"] = "البريد الإلكتروني مستخدم بالفعل";
                        return View(user);
                    }

                    user.Id = Guid.NewGuid();
                    //BoardReq.RegDate = DateTime.Now;
                    int check = _BoardTalRequestsDomain.AddUser(user);
                    if (check == 1)
                        ViewData["Successful"] = "Registeration succ";
                    else
                        ViewData["Falied"] = "Falied";
                    return View(user);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "Falied";
            }

            return View(user);
        }


        public IActionResult Edit(Guid id)
        {
            var user = _BoardTalRequestsDomain.GetTblBoardTalRequestsById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }



    }
}