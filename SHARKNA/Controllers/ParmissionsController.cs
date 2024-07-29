using Microsoft.AspNetCore.Mvc;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security;

namespace SHARKNA.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly PermissionDomain _PermissionDomain;

        public PermissionsController(PermissionDomain PermissionDomain)
        {
            _PermissionDomain = PermissionDomain;
        }
        
        public IActionResult Index()
        {
            var Permission = _PermissionDomain.GetTblPermissions();
            return View(Permission);
        }
        [HttpPost]
        
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PermissionsViewModel Permission)
        {
            if (ModelState.IsValid)
            {
                Permission.Id = Guid.NewGuid();
                _PermissionDomain.AddPermission(Permission);
                return RedirectToAction(nameof(Index));
            }
            return View(Permission);
        }


        public IActionResult Edit(Guid id)
        {
            var Permission = _PermissionDomain.GetTblPermissionsById(id);
            if (Permission == null)
            {
                return NotFound();
            }
            return View(Permission);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PermissionsViewModel Permission)
        {
            if (ModelState.IsValid)
            {
                _PermissionDomain.UpdatePermission(Permission);
                return RedirectToAction(nameof(Index));
            }
            return View(Permission);
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