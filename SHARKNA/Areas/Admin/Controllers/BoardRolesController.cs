using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            try
            {
                if (ModelState.IsValid)
                {

                    boardRoles.Id = Guid.NewGuid();

                    int check = _BoardRolesDomain.AddBoardRoles(boardRoles);
                    // return RedirectToAction(nameof(Index));
                    if (check == 1)
                        ViewData["Successful"] = "تم إضافة  بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ بالإضافة";
                    return View(boardRoles);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إضافة لجنة";
            }

            return View(boardRoles);
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
            try
            {
                if (ModelState.IsValid)
                {

                    int check = _BoardRolesDomain.UpdateBoardRoles(boardRoles);
                    if (check == 1)
                        ViewData["Successful"] = "تم التعديل بنجاح";
                    else
                        ViewData["Falied"] = "خطأ بالتعديل";
                    return View(boardRoles);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ في أثناء التعديل";
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
            return RedirectToAction(nameof(Index), new { Successful, Falied });


        }


    }
}


