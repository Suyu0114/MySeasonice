using Microsoft.EntityFrameworkCore;
using MySeasonice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySeasonice.Data
{
    public class SeaSonicContext : DbContext
    {
        public SeaSonicContext(DbContextOptions<SeaSonicContext> options)
            : base(options)
        {
        }
        public DbSet<MySeasonice.Model.EmpProfile> EmpProfile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmpProfile>().ToTable("EMP_Profile"); // 指定EmpProfile 的Table 名稱
        }

    }
}
