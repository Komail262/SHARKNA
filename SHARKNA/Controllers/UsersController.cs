using Microsoft.AspNetCore.Mvc;
using SHARKNA.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using SHARKNA.Domain;

namespace SHARKNA.Controllers
{
    public class UsersController : Controller
    {
        
        private readonly UserDomain _User;

        public UsersController( UserDomain User)
        {
           
            _User = User;
        }

        public IActionResult Index()
        {
            var users = _User.GettblUsers(); 
            return View(users);
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
