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

            //�ŧi AJAX POST �ϥΪ� Header �W��
            services.AddAntiforgery(o => o.HeaderName = "X-CSRF-TOKEN");
            // Ajax �^��Json �r��
            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            // Ajax �^��Json �j�g�אּ�p�g���D
            services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


            // SQL �s�u  EmpServerContext
            services.AddDbContext<SeaSonicContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EmpServerContext")));

            services.AddTransient<EmpProfileConnectionFactory>(e =>
            {
                //return new EmpProfileConnection(Configuration.GetConnectionString("EmpServerContext"));
                return new EmpProfileConnection(Configuration.GetConnectionString("EmpServerContext"));
            });

            // Add cookie 
            //�q�պAŪ���n�J�O�ɳ]�w
            double LoginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            //���U CookieAuthentication�AScheme����
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //�γ\�n�q�պA��Ū���A�ۤv�r�u�M�w
                option.LoginPath = new PathString("/EmpUser/Login");//�n�J��
                option.LogoutPath = new PathString("/EmpUser/Logout");//�n�XAction

                //�Τ᭶�����d�Ӥ[�A�n�J�O���A��Controller���Τ�n�J�ɾ��I�]�i�H�]�w��
                option.ExpireTimeSpan = TimeSpan.FromMinutes(LoginExpireMinute);//�S���w�]14��
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

            app.UseAuthentication(); // �{�Ҥ覡
            app.UseAuthorization();  // ���v

            # region ���� cookie ���̿���l�ӷ��n�D�B�z����L�������ε{�����w���ʼh��
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // �~��ϥ� Controllers 
                endpoints.MapRazorPages();
            });
        }
    }
}
