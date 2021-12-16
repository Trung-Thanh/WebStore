using eShopSolution.ViewModels.Catalog.forManager;
using FluentValidation;
using System;


namespace eShopSolution.ViewModels.System.SystemValidator
{
    public class ProductRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Bạn cần điền tiên sản phảm")
                .MaximumLength(200).WithMessage("Tên không được phép vượt quá 200 ký tự");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Bạn cần điền giá sản phẩm");

            RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("Bạn cần điền giá sản phẩm giá gốc");

            RuleFor(x => x.Stock).NotEmpty().WithMessage("Bạn cần điền số lượng trong kho");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Bạn cần điền mô tả sản phẩm");

            RuleFor(x => x.Details).NotEmpty().WithMessage("Bạn cần điền chi tiết sản phẩm");

            RuleFor(x => x.SeoDescription).NotEmpty().WithMessage("Bạn cần điền seoDescription của sản phẩm");

            RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("Bạn cần điền seoTitle của sản phẩm");

            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("Bạn cần điền seoAlias của sản phẩm");

            RuleFor(x => x.ThumbnailImage).NotEmpty().WithMessage("Bạn cần tải ảnh cho sản phẩm");
        }
    }
}
