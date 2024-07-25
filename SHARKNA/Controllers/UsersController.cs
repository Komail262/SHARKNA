using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;

namespace SHARKNA.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserDomain _userDomain;

        public UsersController(UserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        public IActionResult Index()
        {
            var users = _userDomain.GetTblUsers();
            return View(users);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_userDomain.IsEmailDuplicate(user.Email))
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل.");
                    return View(user);
                }

                user.Id = Guid.NewGuid(); 
                _userDomain.AddUser(user); 
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

      
        public IActionResult Edit(Guid id)
        {
            var user = _userDomain.GetTblUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_userDomain.IsEmailDuplicate(user.Email, user.Id))
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل.");
                    return View(user);
                }

                _userDomain.UpdateUser(user);
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