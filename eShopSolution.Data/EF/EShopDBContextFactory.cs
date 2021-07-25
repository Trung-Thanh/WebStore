using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.Data.EF
{
    public class EShopDBContextFactory : IDesignTimeDbContextFactory<EShopDBContext>
    {
        // tạo db context, có kiểu đã định nghĩa sẵn
        public EShopDBContext CreateDbContext(string[] args)
        {
            // tao mot doi tuong goc cau hinh
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("eShopSolutionDb"); 

            // mội đối tượng để build db context
            var optionsBuider = new DbContextOptionsBuilder<EShopDBContext>();
            optionsBuider.UseSqlServer(connectionString);

            return new EShopDBContext(optionsBuider.Options);
        }
    }
}
