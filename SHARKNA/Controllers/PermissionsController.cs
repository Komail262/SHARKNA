using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly RoleDomain _RolesDomain;

        public PermissionsController(PermissionDomain permissionDomain, RoleDomain roleDomain)
        {
            _PermissionDomain = permissionDomain;
            _RolesDomain = roleDomain;

        }

        public IActionResult Index()
        {
            var Permission = _PermissionDomain.GetTblPermissions();
            return View(Permission);
        }
        [HttpGet]

        public IActionResult Create()
        {
            ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr");
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            //return View();

            var Permission = await _PermissionDomain.GetTblPermissionsById(id);
            if (Permission == null)
            {
                return NotFound();
            }
            ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr", Permission.RoleId);
            return View(Permission);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PermissionsViewModel Permission)
        {
            ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr", Permission.RoleId);
            if (ModelState.IsValid)
            {
                _PermissionDomain.UpdatePermission(Permission);
                return RedirectToAction(nameof(Index));
            }
            return View(Permission);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}