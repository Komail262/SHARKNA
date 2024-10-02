using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security;
using System.Security.Claims;

namespace SHARKNA.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Super Admin")]
    public class PermissionsController : Controller
    {
        private readonly PermissionDomain _PermissionDomain;
        private readonly RoleDomain _RolesDomain;
        private readonly UserDomain _UserDomain;

        public PermissionsController(PermissionDomain permissionDomain, RoleDomain roleDomain, UserDomain userDomain)
        {
            _PermissionDomain = permissionDomain;
            _RolesDomain = roleDomain;
            _UserDomain = userDomain;

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
        public async Task<IActionResult> Create(PermissionsViewModel permission)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var user = await _PermissionDomain.GetTblUsersByUserName(permission.UserName);
                    if (user == null)
                    {
                        ViewData["Falied"] = "لم يتم العثور على المستخدم";
                        ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr");
                        return View(permission);
                    }


                    bool userHasRole = _PermissionDomain.IsRoleNameDuplicate(permission.UserName);
                    if (userHasRole)
                    {
                        ViewData["Falied"] = "المستخدم لديه صلاحية بالفعل";
                        ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr");
                        return View(permission);
                    }

                    permission.Id = Guid.NewGuid();
                    await _PermissionDomain.AddPermissionAsync(permission, user.UserName);
                    ViewData["Successful"] = "تم تعيين الصلاحية";
                }
                catch (Exception ex)
                {
                    ViewData["Falied"] = "فشل في انشاء صلاحية";
                }
            }
            ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr");
            return View(permission);
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
                ModelState.AddModelError("", "حدث خطأ. حاول مرا اخرى");
                return View(); // or you might want to redirect to an error page
            }


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PermissionsViewModel Permission)
        {
            ViewBag.RolesOfList = new SelectList(_RolesDomain.GetTblRoles(), "Id", "NameAr", Permission.RoleId);

            if (ModelState.IsValid)
            {
                try
                {
                    await _PermissionDomain.UpdatePermissionAsync(Permission, User.FindFirst(ClaimTypes.Name).Value);
                    // return RedirectToAction(nameof(Index));
                    ViewData["Successful"] = "تم التعديل بنجاح";
                }
                catch (Exception ex)
                {
                    // Log the exception (ex)
                    // Consider using a logging framework like NLog, Serilog, or log4net
                    ViewData["Falied"] = "فشل التعديل";
                }
            }

            return View(Permission);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {


                int check = await _PermissionDomain.DeletePermissionAsync(id, User.FindFirst(ClaimTypes.Name).Value);
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

        public async Task<UserViewModel> GetUserInfo(string id)
        {
            return await _UserDomain.GetTblUserByUserName(id);
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