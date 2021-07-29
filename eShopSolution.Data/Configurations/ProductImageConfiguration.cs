using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).UseIdentityColumn();
            builder.Property(x => x.imagePath).HasMaxLength(200).IsRequired();
            builder.Property(x => x.caption).HasMaxLength(200);

            builder.HasOne(x => x.product).WithMany(x => x.productImages).HasForeignKey(x => x.productId);

        }
    }
}
