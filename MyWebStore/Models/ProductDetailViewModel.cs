using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.Category;
using eShopSolution.ViewModels.Catalog.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebStore.Models
{
    public class ProductDetailViewModel
    {
        public CategoryViewModel Category { get; set; }

        public CMProductViewModel Product { get; set; }

        public List<CMProductViewModel> RelatedProducts { get; set; }

        public List<CMProductImageViewModel> ProductIamges { get; set; }
    }
}
