using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.appUser).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);

            //builder.Property(x => x.OrderDate).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.ShipName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ShipAddress).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ShipEmail).IsUnicode(false).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ShipPhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(x => x.Status).HasDefaultValue(OrderStatus.InProgress);
            //throw new NotImplementedException();
        }
    }
}
