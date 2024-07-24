using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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

        // ميثود لعرض النموذج لإدخال مستخدم جديد
        public IActionResult Create()
        {
            return View();
        }

        // ميثود لمعالجة بيانات النموذج وإدخالها في قاعدة البيانات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(tblUsers user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid(); // تعيين معرف جديد
                _userDomain.AddUser(user); // استخدام UserDomain لإضافة المستخدم
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // ميثود لعرض صفحة التحرير
        public IActionResult Edit(Guid id)
        {
            var user = _userDomain.GetTblUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // ميثود لمعالجة بيانات التحرير وتحديثها في قاعدة البيانات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(tblUsers user)
        {
            if (ModelState.IsValid)
            {
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
