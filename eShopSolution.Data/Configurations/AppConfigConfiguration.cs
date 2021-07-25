using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class AppConfigConfiguration : IEntityTypeConfiguration<AppConfig>
    {
        public void Configure(EntityTypeBuilder<AppConfig> builder)
        {
            builder.ToTable("AppConfigs"); 
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Key).UseIdentityColumn();
            builder.Property(x => x.Value).IsRequired(); // mặc định is required là true
            //throw new NotImplementedException();
        }
    }
}
