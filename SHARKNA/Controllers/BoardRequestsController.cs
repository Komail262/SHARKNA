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
        private readonly SHARKNAContext _context;
        public BoardRequestsController (BoardRequestsDomain boardRequestsDomain, SHARKNAContext context)
        {
            _boardRequestsDomain = boardRequestsDomain;
            _context = context;
        }
        public IActionResult Index()
        {
            var users = _boardRequestsDomain.GetTblBoardRequests();
            return View(users);
        }

        public IActionResult Create()
        {
           
            ViewBag.BoardsOfList= new SelectList(ViewBag.BoardsOfList, "Value", "Text");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BoardRequestsViewModel boardRequestsViewModel)
        {
            var selectedValue = boardRequestsViewModel.BoardId;

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
