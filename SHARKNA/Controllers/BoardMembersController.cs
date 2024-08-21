using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;

namespace SHARKNA.Controllers
{
    public class BoardMembersController : Controller
    {
        private readonly BoardMembersDomain _BoardMembersDomain;
        private readonly BoardDomain _boardDomain;
        private readonly SHARKNAContext _context;
       
        public BoardMembersController(BoardMembersDomain Boardmembersdomain, BoardDomain BoardDomain, SHARKNAContext context)
        {
            _BoardMembersDomain = Boardmembersdomain;
            _boardDomain = BoardDomain;
            _context = context;

        }

        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;

             Successful == "20.0" ? "Cold." : "Perfect!";

            var boards = _boardDomain.GetTblBoards();
            return View(boards);
        }

        public IActionResult Members(Guid boardId)
        {
            Guid acceptedStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152");

            var members = _BoardMembersDomain.GetBoardMembersByBoardId(boardId, acceptedStatusId);

            return View(members);
        }

    }


}



