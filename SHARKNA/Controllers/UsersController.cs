using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using SHARKNA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SHARKNA.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;

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

        public IActionResult Edite()
        {
            return View();
        }

        public IActionResult Login()
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
                else if (_userDomain.IsUserNameDuplicate(user.UserName))
                {
                    ModelState.AddModelError("UserName", " اسم المستخدم مستخدم بالفعلا.");
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
                if (_userDomain.IsEmailDuplicate(user.Email, user.Id) )
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل.");
                    return View(user);
                }
               
                 if (_userDomain.IsUserNameDuplicate(user.UserName, user.Id))
                {
                    ModelState.AddModelError("UserName", " اسم المستخدم ,مستخدم بالفعل.");
                    return View(user);

                }

                _userDomain.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            try
            {
                int check = _userDomain.Login(user);

                if (check == 1)
                {
                    // Get user permissions
                    var userPermissions = _userDomain.GetUserByUsername(user.UserName);

                    // Handle the case where the user has no permissions
                    if (userPermissions == null)
                    {
                        // Assign a default role or continue without specific permissions
                        //ViewData["Failed"] = "حدث خطأ في النظام: لم يتم العثور على صلاحيات المستخدم. سيتم تسجيل الدخول كزائر.";

                        userPermissions = new PermissionsViewModel // Use PermissionsViewModel instead
                        {
                            RoleName = "Student", // Default role for regular users
                            Id = Guid.Empty, // Or some default ID
                            FullNameAr = "'طالب'" // Default name for visitor
                        };
                    }

                    var identity = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, userPermissions.RoleName),
                new Claim(ClaimTypes.NameIdentifier, userPermissions.Id.ToString()),
                new Claim(ClaimTypes.GivenName, userPermissions.FullNameAr)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);

                    ViewData["Successful"] = "تم التسجيل الدخول بنجاح";
                    return View();
                }
                else
                {
                    ViewData["Failed"] = "البريد الالكتروني او كلمة المرور غير صحيح";
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here using your logging framework
                ViewData["Failed"] = "حدث خطأ في النظام";
                return View(user);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
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
