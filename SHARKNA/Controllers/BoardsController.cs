﻿using SHARKNA.Models;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SHARKNA.Domain;
using SHARKNA.ViewModels;
using System;

namespace SHARKNA.Controllers
{
    public class BoardsController : Controller
    {
        private readonly BoardDomain _boardDomain;

        public BoardsController(BoardDomain boardDomain)
        {
            _boardDomain = boardDomain;
        }
        public IActionResult Index()
        {
            var boards = _boardDomain.GetTblBoards();
            return View(boards);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BoardViewModel board)
        {
            if (ModelState.IsValid)
            {
                board.Id = Guid.NewGuid();
                _boardDomain.AddBoard(board);
                return RedirectToAction(nameof(Index));
                            
            }
            return View(board);
        }

        public IActionResult Update(Guid id)
        {
            var board = _boardDomain.GetTblBoardById(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BoardViewModel board)
        {
            if (ModelState.IsValid)
            {
                _boardDomain.UpdateBoard(board);
                return RedirectToAction(nameof(Index));
            }
            return View(board);
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
