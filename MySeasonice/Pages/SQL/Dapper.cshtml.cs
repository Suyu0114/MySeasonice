using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySeasonice.Data;
using Dapper;
using MySeasonice.Model;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Data.SqlClient;

namespace MySeasonice.Pages.SQL
{
    public class DapperModel : PageModel
    {
        //public readonly MySeasonice.Data.EmpProfileConnection _conn;
        private readonly IConfiguration Configuration;

        public DapperModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<EmpProfile> EmpProfile { get; set; }

        public class ConnectionOptions
        {
            public const string Position = "ConnectionStrings";
            public string EmpServerContext { get; set; }
        }

        public void OnGet() 
        {
            var conn = new ConnectionOptions();

            Configuration.GetSection(ConnectionOptions.Position).Bind(conn);

            // 檢查連線字串
            //using (var connection = new SqlConnection(conn.EmpServerContext))
            //{
            //    return Content(connection.GenerateAllTables());
            //};
            //return Content("config: " + conn.EmpServerContext);

            using (var connection = new SqlConnection(conn.EmpServerContext))
            {
                //生成 View
                var result = connection.GenerateAllTables(GeneratorBehavior.View);
                Console.WriteLine(result);

                // 產生class
                result = result = connection.GenerateAllTables(GeneratorBehavior.Comment);
                Console.WriteLine(result);

                // Select 

            }

            using (var con = new SqlConnection(conn.EmpServerContext))
            {
                EmpProfile = con.Query<EmpProfile>("select * from EMP_Profile ").ToList();
            }

            Console.WriteLine(EmpProfile);

        }

        public async Task<IEnumerable<EmpProfile>> ListInvoices()
        {
            var conn = new ConnectionOptions();
            Configuration.GetSection(ConnectionOptions.Position).Bind(conn);

            using (var connection = new SqlConnection(conn.EmpServerContext))
            {
                //var parameters = new { Skip = (page - 1) * pageSize, Take = pageSize };

                var query = @"select * from EMP_Profile ";

                //return await connection.QueryAsync<EmpProfile>(query, parameters);
                return (await connection.QueryAsync<EmpProfile>(query)).ToList();
            }
        }


        //public async Task OnGetAsync()
        //{

        //    //var thisEmp = await GetEmpProfiles();
        //    Console.WriteLine(thisEmp);
        //}

        //public async Task<IEnumerable<EmpProfile>> GetEmpProfiles()
        //{
        //    var cs = "Data Source=192.168.2.61;User ID=sa;Password=DSC@dsc;Database=EmpServer;MultipleActiveResultSets=true";
        //    //using var conn = await _conn.CreateConnectionAsync();
        //    using var conn = new SqlServerConnection(cs);


        //    var result = conn.Query<EmpProfile>(" Select * FROM EMP_Profile").ToList();

        //    return result;
        //}

        //public static GetConnectionString() 
        //{
        //    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        //    var config = builder.Build();
        //    var connectionString = config.GetConnectionString("EmpServerContext");
        //}
    }

}
