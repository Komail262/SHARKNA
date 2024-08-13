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

        public IActionResult Index(string Successful = "", string Falied = "")
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;

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
                try
                {
                    bool userHasRole = _PermissionDomain.IsRoleNameDuplicate(Permission.UserName);
                    if (userHasRole)
                    {
           
                        ViewData["Falied"] = "The user already has a role .";
                        ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr");
                        return View(Permission);
                    }

                    Permission.Id = Guid.NewGuid();
                    _PermissionDomain.AddPermission(Permission);
                    ViewData["Successful"] = "Successful";
                }
                catch (Exception ex)
                {
                    ViewData["Falied"] = "Falied";
                }
            }
            return View(Permission);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            //return View()
                try
                {
                    var Permission = await _PermissionDomain.GetTblPermissionsById(id);
                    if (Permission == null)
                    {

                        return NotFound();
                    }

                    ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr", Permission.RoleId);
                    return View(Permission);
                }
                catch (Exception ex)
                {
                    // Log the exception (ex)
                    // Consider using a logging framework like NLog, Serilog, or log4net
                    ModelState.AddModelError("", "An error occurred while retrieving the permission. Please try again.");
                    return View(); // or you might want to redirect to an error page
                }
            

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PermissionsViewModel Permission)
        {
            ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr", Permission.RoleId);

            if (ModelState.IsValid)
            {
                try
                {
                    _PermissionDomain.UpdatePermission(Permission);
                   // return RedirectToAction(nameof(Index));
                    ViewData["Successful"] = "Successful";
                }
                catch (Exception ex)
                {
                    // Log the exception (ex)
                    // Consider using a logging framework like NLog, Serilog, or log4net
                    ViewData["Falied"] = "Falied";
                }
            }

            return View(Permission);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {


                int check = _PermissionDomain.DeletePermission(id);
                if (check == 1)
                {
                    Successful = "تم حذف الصلاحية";
                }

                else
                {
                    Falied = "حدث خطأ";


                }
                //return View(id);

            }
            catch (Exception ex)
            {
                Falied = "حدث خطأ";

            }
            //_boardDomain.DeleteBoard(id);
            return RedirectToAction(nameof(Index), new { Successful = Successful, Falied = Falied });
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