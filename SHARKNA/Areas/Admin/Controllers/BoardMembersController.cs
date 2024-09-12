using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BoardMembersController : Controller
    {
        private readonly BoardMembersDomain _BoardMembersDomain;
        private readonly BoardDomain _boardDomain;
        private readonly BoardRolesDomain _boardRolesDomain;
        private readonly UserDomain _UserDomain;
        private readonly SHARKNAContext _context;

        public BoardMembersController(BoardMembersDomain Boardmembersdomain, BoardDomain BoardDomain, SHARKNAContext context, BoardRolesDomain boardRolesDomain, UserDomain userDomain)
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




        public async Task<IActionResult> Members(Guid boardId, string Successful = "", string Falied = "")
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;
            var members = await _BoardMembersDomain.GetBoardMembersByBoardId(boardId);

            return View(members);

            //return View(await _BoardMembersDomain.GetBoardMembersByBoardId(boardId));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var MemR = await _BoardMembersDomain.GetBoardMemberByIdAsync(id);
            ViewBag.BoardRoleOfList = new SelectList(_boardRolesDomain.GettbBoardRoles(), "Id", "NameAr", MemR.BoardRoleId);
            return View(MemR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(tblBoardMembers MemR)
        {
            try
            {
                int check = await _BoardMembersDomain.UpdateBoardMembersAsync(MemR);
                if (check == 1)
                    ViewData["Successful"] = "تم التعديل بنجاح";

                else
                    ViewData["Falied"] = "حدث خطأ أثناء معالجتك طلبك الرجاء المحاولة في وقت لاحق";
                ViewBag.BoardRoleOfList = new SelectList(_boardRolesDomain.GettbBoardRoles(), "Id", "NameAr");
            }
            catch
            {
                ViewData["Falied"] = "حدث خطأ أثناء معالجتك طلبك الرجاء المحاولة في وقت لاحق";
            }

            return RedirectToAction("Members", new { boardId = MemR.BoardId });
        }




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
            return RedirectToAction(nameof(Members));
        }


    }


}



