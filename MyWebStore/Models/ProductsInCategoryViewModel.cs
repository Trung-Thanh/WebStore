using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.Category;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebStore.Models
{
    public class ProductsInCategoryViewModel
    {
        public CategoryViewModel Category { get; set; }

        public PageResult<CMProductViewModel> Products { get; set; }
    }
}
