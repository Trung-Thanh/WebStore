using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eShopSolution.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // this is called "body expresstion" coding style
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // use microsoft.extention.hosting class create a default builder with args (arguments) and add configurations in start up
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // cau hinh luu trong file startup
                    webBuilder.UseStartup<Startup>();
                });
    }
}
