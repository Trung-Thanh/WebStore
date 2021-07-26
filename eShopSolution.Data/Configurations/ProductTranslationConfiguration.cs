using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.ToTable("ProductTranslations");
            builder.HasKey(x => new { x.ProductId, x.LanguageId });

            builder.Property(x => x.LanguageId).IsUnicode(false).IsRequired().HasMaxLength(5);

            builder.HasOne(x => x.Product).WithMany(x => x.productTranslations).HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.Language).WithMany(x => x.productTranslations).HasForeignKey(x => x.LanguageId);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.SeoAlias).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Details).HasMaxLength(500);

            // thống nhất là language id có dội dài 5, nếu k giống nhau sẽ không tạo được khóa ngoại
            

            //throw new NotImplementedException();
        }
    }
}
