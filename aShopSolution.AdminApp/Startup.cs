using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aShopSolution.AdminApp.Service;
using eShopSolution.ViewModels.System.User;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace aShopSolution.AdminApp
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
            services.AddHttpClient();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login/";
                    options.AccessDeniedPath = "/User/Forbidden/";
                });

            services.AddControllersWithViews()
                     .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            // add session for 30 minute
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            // add service api
            services.AddTransient<IUserApiClient, UserApiClient>();

            // does not bien dich again
            IMvcBuilder builder = services.AddRazorPages();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
