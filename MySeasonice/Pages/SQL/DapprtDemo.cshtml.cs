
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

using MySeasonice.Model;

namespace MySeasonice.Pages.SQL
{
    public class DapprtDemoModel : PageModel
    {
        private readonly IConfiguration Configuration;
        public DapprtDemoModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public class ConnectionOptions
        {
            public const string Position = "ConnectionStrings";
            public string EmpServerContext { get; set; }
        }

        public List<EmpProfile> EmpProfile { get; set; }
        public void OnGet()
        {
			var conn = new ConnectionOptions();
			Configuration.GetSection(ConnectionOptions.Position).Bind(conn);

            string sqlStr = " select * from EMP_Profile ";

            using (var con = new SqlConnection(conn.EmpServerContext))
            {
                EmpProfile = con.Query<EmpProfile>(sqlStr).ToList();
            }

			//EmpProfile = con.Query<EmpProfile>("select * from EMP_Profile ").ToList();
        }

    }
}
