using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SHARKNA.Controllers
{
    public class BoardRolesController : Controller
    {
        private readonly BoardRolesDomain _BoardRolesDomain;

        public BoardRolesController(BoardRolesDomain boardRoles)
        {
            _BoardRolesDomain = boardRoles;
        }

        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;

            var boardRoles = _BoardRolesDomain.GettbBoardRoles();
            return View(boardRoles);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BoardRolesViewModel boardRoles)
        {
            if (ModelState.IsValid)
            {
               boardRoles.Id = Guid.NewGuid();
                _BoardRolesDomain.AddBoardRoles(boardRoles);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public IActionResult Edit(Guid id)
        {

            var boardRoles = _BoardRolesDomain.GetTblBoardRolesById(id);
            if (boardRoles == null)
            {
                return NotFound();
            }
            return View(boardRoles);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BoardRolesViewModel boardRoles)
        {
            if (ModelState.IsValid)
            {
                _BoardRolesDomain.UpdateBoardRoles(boardRoles);

                return RedirectToAction(nameof(Index));
            }
            return View(boardRoles);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {


                int check = _BoardRolesDomain.DeleteBoaRoles(id);
                if (check == 1)
                {
                    Successful = "تم حذف  بنجاح";
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
            //_boardDomain.DeleteBoard(id);
            return RedirectToAction(nameof(Index), new { Successful = Successful, Falied = Falied });


        }


    }
}


