using eShopColution.Utilities.Constants;
using eShopSolution.Appication.Catalog.products;
using eShopSolution.Appication.Common;
using eShopSolution.Appication.System.User;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace eShopSolution.WebApi
{
    public class Startup
    {
        // 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) // for service
        {
            // get table form from class EShopDBContext, then use connection string to connect to the db on SqlServer
            services.AddDbContext<EShopDBContext>(options=>options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.mainConllectionString)));
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<EShopDBContext>().AddDefaultTokenProviders();

            // declare interface and implement class to work with db
            // o guest that the Context will be add into PublicProductService constrctor in this step
            //Declare DI
            services.AddTransient<IStorageService, FileStorageService>();

            services.AddTransient<IPublicProductService, PublicProductService>();
            services.AddTransient<IManageProductSevice, ManageProductSevice>();

            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient<IUserService, UserService>();


            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShop Solution", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // for middleware
        {
            if (env.IsDevelopment())
            {
                // if is runing on dev environment, usr dev exceprionpage,  that page will show us with exceptions
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eShopSolution V1");
            });

            // có thể mặc định là trang cuối cùng của rewquet là home
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
