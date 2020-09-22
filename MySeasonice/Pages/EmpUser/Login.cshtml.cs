using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySeasonice.Data;
using MySeasonice.Lib;

namespace MySeasonice.Pages.EmpUser
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration Configuration;
        public LoginModel(ILogger<LoginModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<InputModel> Input_conn { get; set; }
        public string ReturnUrl { get; set; }
        public string Message { get; set; }
        public class InputModel
        {
            [Required]
            public string ID { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            public string FullName { get; set; }
            public string FST_Name { get; set; }
            public string Role { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                ModelState.AddModelError(string.Empty, Message);
            }

            // Clear the existing external cookie
            #region snippet2
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            ReturnUrl = returnUrl;

        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            ReturnUrl = returnUrl;

            await Task.Delay(200);

            if (ModelState.IsValid)
            {
                //var user = AuthenticateUser(Input.ID, Input.Password);

                if (!LDAPUtil.Validate(Input.ID, Input.Password)) //驗證失敗
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                // conn 取的登入者資料
                var conn = new DapperConnection.ConnectionOptions();
                Configuration.GetSection(DapperConnection.ConnectionOptions.Position).Bind(conn);

                string sqlStr = string.Format(@" SELECT TOP 1 
                                        '{0}' as ID,
                                        FST_Name + ' ' + LST_Name as FullName ,
                                        case ISNULL(FST_Name, '')
	                                    when '' then ISNULL(LST_Name, '')
	                                    else FST_Name end FST_Name
                                        from EMP_Profile where EMP_Account like '%{0}%'", Input.ID);

                using (var con = new Microsoft.Data.SqlClient.SqlConnection(conn.EmpServerContext))
                {
                    Input_conn = con.Query<InputModel>(sqlStr).ToList();
                    //List_EmpProfile = con.Query<EmpProfile>(sqlStr, new { OrderDetailID = 1 }).ToList();
                }


                // 之後建立員工資料表 再加上 EMP info 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Input.ID),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim("FstName", Input_conn[0].FST_Name),
                    new Claim(ClaimTypes.Surname, Input_conn[0].FullName),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {

                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation("User {Email} logged in at {Time}.",
                    Input.ID, DateTime.UtcNow);

                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }
            // Something failed. Redisplay the form.
            return Page();
        }

    }
}
