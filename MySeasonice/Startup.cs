using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MySeasonice.Data;

namespace MySeasonice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //宣告 AJAX POST 使用的 Header 名稱
            services.AddAntiforgery(o => o.HeaderName = "X-CSRF-TOKEN");
            // Ajax 回傳Json 字串
            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            // Ajax 回傳Json 大寫改為小寫問題
            services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


            // SQL 連線  EmpServerContext
            services.AddDbContext<SeaSonicContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EmpServerContext")));

            services.AddTransient<EmpProfileConnectionFactory>(e =>
            {
                //return new EmpProfileConnection(Configuration.GetConnectionString("EmpServerContext"));
                return new EmpProfileConnection(Configuration.GetConnectionString("EmpServerContext"));
            });

            // Add cookie 
            //從組態讀取登入逾時設定
            double LoginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            //註冊 CookieAuthentication，Scheme必填
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //或許要從組態檔讀取，自己斟酌決定
                option.LoginPath = new PathString("/EmpUser/Login");//登入頁
                option.LogoutPath = new PathString("/EmpUser/Logout");//登出Action

                //用戶頁面停留太久，登入逾期，或Controller中用戶登入時機點也可以設定↓
                option.ExpireTimeSpan = TimeSpan.FromMinutes(LoginExpireMinute);//沒給預設14天
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // 認證方式
            app.UseAuthorization();  // 授權

            # region 提高 cookie 不依賴跨原始來源要求處理的其他類型應用程式的安全性層級
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // 才能使用 Controllers 
                endpoints.MapRazorPages();
            });
        }
    }
}
