using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SHARKNA.Controllers
{
    public class BoardMembersController : Controller
    {
        private readonly BoardMembersDomain _BoardMembersDomain;
        private readonly BoardDomain _boardDomain;
        private readonly BoardRolesDomain _boardRolesDomain;
        private readonly UserDomain _UserDomain;
        private readonly SHARKNAContext _context;

        public BoardMembersController(BoardMembersDomain Boardmembersdomain, BoardDomain BoardDomain, SHARKNAContext context, BoardRolesDomain boardRolesDomain ,UserDomain userDomain)
        {
            _BoardMembersDomain = Boardmembersdomain;
            _boardDomain = BoardDomain;
            _context = context;
            _boardRolesDomain = boardRolesDomain;
            _UserDomain = userDomain;
        }

        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;

            var boards = _boardDomain.GetTblBoards();
            return View(boards);
        }

        //public async Task<IActionResult> Details(Guid id)
        //{
        //    var username = User.FindFirst(ClaimTypes.Name)?.Value;
        //    var user = _UserDomain.GetUserFER(username);

        //    var boardMem = await _BoardMembersDomain.GetBoardMemberById(id); 
        //    if (boardMem == null)
        //    {
        //        return NotFound();
        //    }
        //    //var model = new BoardMembersViewModel
        //    //{
        //    //    UserName = username,
        //    //    Email = user.Email,
        //    //    MobileNumber = user.MobileNumber,
        //    //    FullNameAr = user.FullNameAr,
        //    //    FullNameEn = user.FullNameEn
        //    //};

        //    ViewBag.BoardRoleOfList = new SelectList(_boardRolesDomain.GettbBoardRoles(), "Id", "NameAr");
        //    return View(boardMem);
        //}


        public async Task<IActionResult> Members(Guid boardId, string Successful = "", string Falied = "")
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;


            return View(await _BoardMembersDomain.GetBoardMembersByBoardId(boardId));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var MemR = await _BoardMembersDomain.GetBoardMemberByIdAsync(id);
            ViewBag.BoardRoleOfList = new SelectList(_boardRolesDomain.GettbBoardRoles(), "Id", "NameAr",MemR.BoardRoleId);
            return View(MemR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(tblBoardMembers MemR)
        {
            try
            {
                int check = _BoardMembersDomain.UpdateBoardMembersAsync(MemR);
                if (check == 1)
                    ViewData["Successful"] = "تم التعديل بنجاح";
               
                else
                    ViewData["Falied"] = "حدث خطأ أثناء معالجتك طلبك الرجاء المحاولة في وقت لاحق";
            }
            catch
            {
                ViewData["Falied"] = "حدث خطأ أثناء معالجتك طلبك الرجاء المحاولة في وقت لاحق";
            }
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Details(BoardMembersViewModel boardMem)
        //{
        //    var username = User.FindFirst(ClaimTypes.Name)?.Value;
        //    var user = _UserDomain.GetUserFER(username);

        //    if (ModelState.IsValid)
        //    {
        //        int check = await _BoardMembersDomain.UpdateBoardMembersAsync(boardMem);

        //        //boardMem.Id = Guid.NewGuid();
        //        //boardMem.UserName = username;
        //        //boardMem.Email = user.Email;
        //        //boardMem.MobileNumber = user.MobileNumber;
        //        //boardMem.FullNameAr = user.FullNameAr;
        //        //boardMem.FullNameEn = user.FullNameEn;

        //        if (check == 1)
        //        {
        //            ViewData["Successful"] = "تم التعديل بنجاح";
        //            return RedirectToAction(nameof(Members));
        //        }
        //        else
        //        {
        //            ViewData["Falied"] = "خطأ بالتعديل";
        //        }
        //    }
        //    ViewBag.BoardRoleOfList = new SelectList(_boardRolesDomain.GettbBoardRoles(), "Id", "NameAr");
        //    return View(boardMem);
        //}



        [HttpGet]
   
        public async Task<IActionResult> Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {


                int check = await _BoardMembersDomain.DeleteBoardMembersAsync(id);
                if (check == 1)
                {
                    Successful = "تم حذف اللجنة بنجاح";
                }

                else
                {
                    Falied = "حدث خطأ";


                }

            }
            catch (Exception ex)
            {
                Falied = "حدث خطأ";

            }
            return RedirectToAction(nameof(Members), new { Successful = Successful, Falied = Falied });
        }


    }


}



