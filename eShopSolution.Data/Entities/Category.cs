using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public bool IsShowOnHome { get; set; }
        public int?  ParentID { get; set; }
        public Status Status { get; set; }
        
        // thể hiện một thể loại thì có một list nhưng sản phẩm củ thệ thuộc về loại đó
        public List<ProductsInCategories> ProductsInCategories { get; set; }

        public List<CategoryTranslation> categoryTranslations { get; set; }
    }
}
