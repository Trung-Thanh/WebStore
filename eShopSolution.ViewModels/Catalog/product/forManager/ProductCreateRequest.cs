using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.forManager
{
    public class ProductCreateRequest
    {
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { get; set; }

        [Display(Name = "Số lượng")]
        public int Stock { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Mô tả")]
        public string Description { set; get; }

        [Display(Name = "Chi tiết")]
        public string Details { set; get; }

        public string SeoDescription { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        public string LanguageId { set; get; }

        [Display(Name = "Có phải sản phẩm nổi bật ?")]
        public bool? IsFeature { get; set; }


        [Display(Name = "Ảnh minh họa")]
        public  IFormFile ThumbnailImage { get; set; }

    }
}
