﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BoardsController : Controller
    {
        private readonly BoardDomain _boardDomain;
        private readonly UserDomain _UserDomain;


        public BoardsController(BoardDomain boardDomain, UserDomain userDomain)
        {
            _boardDomain = boardDomain;
            _UserDomain = userDomain;
        }

        [Authorize(Roles = "Admin,Super Admin,Editor")]
        public IActionResult Index(string Successful = "", string Falied = "")
        {

            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;



            var boards = _boardDomain.GetTblBoards();
            return View(boards);
        }
        [Authorize(Roles = "Admin,Super Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Super Admin,Editor")]
        public async Task<IActionResult> Update(Guid id)
        {
            var board = await _boardDomain.GetTblBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }
        [Authorize(Roles = "Admin,Super Admin,Editor")]
        public async Task<IActionResult> Details(Guid id)
        {
            var board = await _boardDomain.GetTblBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardViewModel board)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //if (await _boardDomain.IsBoardNameDuplicateAsync(board.NameEn))
                    //{
                    //    ViewData["Falied"] = "اسم اللجنة مستخدم بالفعل";
                    //    return View(board);
                    //}

                    board.Id = Guid.NewGuid();

                    string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims
                    //string fullNameAr = await _boardDomain.GetFullNameArByUsernameAsync(username);


                    int check = await _boardDomain.AddBoardAsync(board, username); // Pass the username and board to the domain method
                    if (check == 1)
                        ViewData["Successful"] = "تم إضافة اللجنة بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ بالإضافة";
                    return View(board);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إضافة لجنة";
            }

            return View(board);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Update(BoardViewModel board)
        {
            try
            {
                if (ModelState.IsValid)
                {



                    var Eboard = await _boardDomain.GetTblBoardByIdAsync(board.Id);//ياخذ بيانات اللجنة الحالموجودة حاليا من الداتابيس باستخدام ID قبل اي تعديل  


                    string username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from claims

                    int check = await _boardDomain.UpdateBoardAsync(board, username);
                    if (check == 1)
                        ViewData["Successful"] = "تم التعديل بنجاح";
                    else
                        ViewData["Falied"] = "خطأ بالتعديل";
                    return View(board);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء التعديل";
            }
            return View(board);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid boardId)
        {
            try
            {
                string username = User.FindFirst(ClaimTypes.Name)?.Value;

                await _boardDomain.DeleteBoardAsync(id, username);
                ViewData["Successful"] = "تم حذف اللجنة بنجاح";
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء محاولة الحذف .";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
