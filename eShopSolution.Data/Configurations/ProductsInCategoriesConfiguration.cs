using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    class ProductsInCategoriesConfiguration : IEntityTypeConfiguration<ProductsInCategories>
    {
        public void Configure(EntityTypeBuilder<ProductsInCategories> builder)
        {
            //throw new NotImplementedException();
            builder.ToTable("ProductsInCategories");

            // phải tao một object làm định danh - khóa chính bằng toán tử new
            builder.HasKey(t => new { t.ProductId, t.CategoryId });

            // hai khóa ngoại
            // thứ 1
            // t de la bien cuc bo
            builder.HasOne(t => t.Product)
                   // ProductsInCategories la thuoc tinh lop Product
                   .WithMany(t=>t.ProductsInCategories)
                   // ProductId la thuoc tinh cua lop ProductsInCategories 
                   .HasForeignKey(t => t.ProductId);

            // thứ 2
            builder.HasOne(t => t.Category)
                   .WithMany(t => t.ProductsInCategories)
                   .HasForeignKey(t => t.CategoryId);
        }
    }
}
