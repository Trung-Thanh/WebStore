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
                // giải quyết câu hỏi tạo ở đâu
                .AddJsonFile("appsettings.json")
                .Build();

            // lay cu the thng qua tu khoa
            var connectionString = configuration.GetConnectionString("eShopSolutionDb");

            // mội đối tượng để build db context voi loi ke thua IdentityDbContext là "EShopDBContext"
            var optionsBuider = new DbContextOptionsBuilder<EShopDBContext>();
            // sử dụng sqlserver để sinh db
            optionsBuider.UseSqlServer(connectionString);

            return new EShopDBContext(optionsBuider.Options);
        }
    }
}
