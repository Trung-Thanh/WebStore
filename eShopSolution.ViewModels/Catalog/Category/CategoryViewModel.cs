using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { set; get; }
        public int CategoryLv { get; set; }
    }
}
