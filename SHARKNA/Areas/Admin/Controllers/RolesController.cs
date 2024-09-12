using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Data;
using System.Diagnostics;
using System.Security;

namespace SHARKNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleDomain _RoleDomain;

        public RolesController(RoleDomain RoleDomain)
        {
            _RoleDomain = RoleDomain;

        }

        public IActionResult Index()
        {
            var Role = _RoleDomain.GetTblRoles();
            return View(Role);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RolesViewModel Role)
        {
            if (ModelState.IsValid)
            {
                Role.Id = Guid.NewGuid();
                _RoleDomain.AddRole(Role);
                return RedirectToAction(nameof(Index));
            }
            return View(Role);
        }


        public IActionResult Edit(Guid id)
        {
            var Role = _RoleDomain.GetTblRolesById(id);
            if (Role == null)
            {
                return NotFound();
            }
            return View(Role);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RolesViewModel Role)
        {
            if (ModelState.IsValid)
            {
                _RoleDomain.UpdateRoles(Role);
                return RedirectToAction(nameof(Index));
            }
            return View(Role);
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