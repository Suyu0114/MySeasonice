using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySeasonice.Model;
using Dapper;
using MySeasonice.Data;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MySeasonice.Pages
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        private readonly IConfiguration Configuration;
        public IndexModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [BindProperty]
        public string EMP_SearchString { get; set; }
        public string UserName_Claim { get; set; }
        public string FstName { get; set; }


        public List<EmpProfile> List_EmpProfile { get; set; }
        public void OnGet()
        {
            List_EmpProfile = new List<EmpProfile>();

            // 取得 Claim 資料的方法 
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            UserName_Claim = claimsIdentity.FindFirst(ClaimTypes.Surname)?.Value;
            FstName = claimsIdentity.FindFirst("FstName")?.Value;
        }
        //[HttpPost]
        public IActionResult OnPostEMP_Search()
        {
            var conn = new DapperConnection.ConnectionOptions();
            Configuration.GetSection(DapperConnection.ConnectionOptions.Position).Bind(conn);

            string sqlStr = " select * from EMP_Profile ";

            if (!string.IsNullOrEmpty(EMP_SearchString))
            {
                sqlStr += string.Format(" where EMP_Account like '%{0}%' ", EMP_SearchString);
            }

            using (var con = new Microsoft.Data.SqlClient.SqlConnection(conn.EmpServerContext))
            {
                List_EmpProfile = con.Query<EmpProfile>(sqlStr).ToList();
                //List_EmpProfile = con.Query<EmpProfile>(sqlStr, new { OrderDetailID = 1 }).ToList();
            }

            return new JsonResult(List_EmpProfile);
        }
    }

}
