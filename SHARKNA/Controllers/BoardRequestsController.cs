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


        public BoardRequestsController (BoardRequestsDomain boardRequestsDomain, BoardDomain BoardDomain, RequestStatusDomain requestStatusDomain)
        {
            _boardRequestsDomain = boardRequestsDomain;
            _BoardDomain = BoardDomain;
            _RequestStatusDomain = requestStatusDomain;
        }
        public IActionResult Index()
        {
            var BoardReq = _boardRequestsDomain.GetTblBoardRequests();
            return View(BoardReq);
        }

        public IActionResult Create()
        {
           
            ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr");
           \


            return View();
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
                    //BoardReq.RegDate = DateTime.Now;
                   int check =  _boardRequestsDomain.AddBoardReq(BoardReq);
                    if (check == 1)
                        ViewData["Successful"] = "Registeration succ";
                    else
                        ViewData["Falied"] = "Falied";
                    return View(BoardReq);
                    
                }
            }
            catch (Exception ex) {
                ViewData["Falied"] = "Falied";
            }

            return View(BoardReq);
        }



        public IActionResult Edit(Guid id)
        {
            var BoardReq = _boardRequestsDomain.GetTblBoardRequestsById(id);
            if (BoardReq == null)
            {
                return NotFound();
            }
            return View(BoardReq);
        }


        }

    }

