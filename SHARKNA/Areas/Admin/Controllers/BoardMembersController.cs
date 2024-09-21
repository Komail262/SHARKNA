using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin,SuperAdmin,Editor")]

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
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims

                int check = await _BoardMembersDomain.UpdateBoardMembersAsync(MemR, username);

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
            return RedirectToAction("Members", "BoardMembers", new { boardId = MemR.BoardId });

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid boardId)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims


                await _BoardMembersDomain.DeleteBoardMembersAsync(id, username);
                //await _BoardMembersDomain.DeleteBoardMembersAsync(id);
                ViewData["Successful"] = "تم حذف العضو بنجاح";
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء محاولة الحذف .";
            }
                return RedirectToAction(nameof(Members), new { boardId = boardId });
        }

       


    }


}



