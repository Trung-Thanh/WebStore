using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(x => new { x.OrderId, x.ProductId });
            builder.Property(x => x.Price).IsRequired();

            builder.HasOne(x=>x.Order)
                    .WithMany(x=>x.OrderDetails)
                    .HasForeignKey(x=>x.OrderId);

            builder.HasOne(x => x.Product)       // x lay dai dien cho Orderdetail
                .WithMany(x => x.orderDetails)   // x nay dai dien cho product
                .HasForeignKey(x => x.ProductId);// x lay dai dien cho Orderdetail

            //throw new NotImplementedException();
        }
    }
}
