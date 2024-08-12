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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SHARKNA.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserDomain _userDomain;

        public UsersController(UserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = _userDomain.GetTblUsers();
            return View(users);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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
        
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                
                var userRecord = _userDomain.GetUserByUsernameAndPassword(user.UserName , user.Password);

                if (userRecord != null)
                {
                    
                    var userPermissions = _userDomain.GetUserByUsername(user.UserName);

                   
                    if (userPermissions == null)
                    {
                        userPermissions = new PermissionsViewModel
                        {
                            RoleName = "NoRole",
                            Id = userRecord.Id,  
                            FullNameAr = userRecord.FullNameAr  
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

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ViewData["Login_error"] = "حدث خطأ في النظام";
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
