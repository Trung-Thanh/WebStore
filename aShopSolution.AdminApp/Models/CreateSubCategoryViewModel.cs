using eShopSolution.ViewModels.Catalog.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aShopSolution.AdminApp.Models
{
    public class CreateSubCategoryViewModel
    {
        public CUCategoryRequest ucCategoryRequest { get; set; }

        public List<CategoryViewModel> ListParents { get; set; }
    }
}
