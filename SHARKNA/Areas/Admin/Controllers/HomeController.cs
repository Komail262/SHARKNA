using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using System.Diagnostics;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly SHARKNAContext _context;

        public HomeController(SHARKNAContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            var userCount = GetUserCount();
            var boardCount = GetBoardCount();
            var boardMemberCount = GetBoardMemberCount();

            ViewBag.UserCount = userCount;
            ViewBag.BoardCount = boardCount;
            ViewBag.BoardMemberCount = boardMemberCount;


            return View();
        }

        private int GetUserCount()
        {
            return _context.tblUsers.Count(); // Adjusted for no IsDeleted field
        }

        private int GetBoardCount()
        {
            return _context.tblBoards.Count(b => !b.IsDeleted); // Assuming IsDeleted is in tblBoards
        }

        private int GetBoardMemberCount()
        {
            return _context.tblBoardMembers.Count(bm => !bm.IsDeleted); // Assuming IsDeleted is in tblBoardMembers
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
