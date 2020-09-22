using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MySeasonice.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using Dapper;

namespace MySeasonice.Model
{

    public class EmpProfile
    {
        public class SeaSonicContext : DbContext
        {
            public SeaSonicContext(DbContextOptions<SeaSonicContext> options)
                : base(options)
            {
            }
            public DbSet<MySeasonice.Model.EmpProfile> EmpProfile { get; set; }
        }
        [Display(Name = "Dep ID")]
        public string Dep_Id { get; set; }
        public string Enable { get; set; }
        public string Departmen_TC { get; set; }
        public string Department { get; set; }
        [Key] // SQL 主Key
        [Display(Name = "No.")]
        public string EMP_Id { get; set; }
        [Display(Name = "Account Name")]
        public string EMP_Account { get; set; }
        public string FST_Name { get; set; }
        public string LST_Name { get; set; }
        [Display(Name = "Local Name")]
        public string EMP_Name_TC { get; set; }
        public string JobTitle_ID { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle_TC { get; set; }
        public string JobTitle { get; set; }

        [Display(Name = "Office Phone")]
        public string Office_phone { get; set; }
        public string Email { get; set; }
        public string Manager_Id { get; set; }
        public string Manager2nd_Id { get; set; }


    }

    // Program 使用

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SeaSonicContext(
                serviceProvider.GetRequiredService<DbContextOptions<SeaSonicContext>>()))
            {
                // Look for any EmpProfiles.
                if (context.EmpProfile.Any())
                {
                    return;   // DB has been seeded
                }

                context.EmpProfile.AddRange(); //如果Table 為空，可以在此塞入值
                context.SaveChanges();
            }

        }
    }
}
