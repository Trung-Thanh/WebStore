using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Appication.Catalog.products.DataTransferObject.forManager
{
    public class ProductUpdateRequest
    {
        public int id { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }
    }
}
