using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MySeasonice.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User {Name} logged out at {Time}.",
                User.Identity.Name, DateTime.UtcNow);

            #region snippet1
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            return RedirectToPage("/EmpUser/Login");
        }

        public IActionResult UserName()
        {
            ViewData["Hello"] = "Hello Suyu ";

            return View();
        }
    }
}
