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

        public BoardRequestsController (BoardRequestsDomain boardRequestsDomain, BoardDomain BoardDomain)
        {
            _boardRequestsDomain = boardRequestsDomain;
            _BoardDomain = BoardDomain;
        }
        public IActionResult Index()
        {
            var users = _boardRequestsDomain.GetTblBoardRequests();
            return View(users);
        }

        public IActionResult Create()
        {
           
            ViewBag.BoardsOfList = new SelectList(_BoardDomain.GetTblBoards(), "Id", "NameAr");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create(BoardRequestsViewModel boardRequestsViewModel)
        //{
        //    var selectedValue = boardRequestsViewModel.BoardId;

        //    return View(boardRequestsViewModel);
        //}

        public IActionResult Create(BoardRequestsViewModel boardRequestsViewModel)
        {
            if (ModelState.IsValid)
            {
                boardRequestsViewModel.Id = Guid.NewGuid();
                _boardRequestsDomain.AddBoardReq(boardRequestsViewModel);
                return RedirectToAction(nameof(Index));

            }
            return View(boardRequestsViewModel);
        }



        public IActionResult Edit(Guid id)
        {
            var user = _boardRequestsDomain.GetTblBoardRequestsById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BoardRequestsViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_boardRequestsDomain.IsEmailDuplicate(user.Email, user.Id))
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل.");
                    return View(user);
                }

                _boardRequestsDomain.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
