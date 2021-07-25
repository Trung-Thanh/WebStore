using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class CategoryTranslation
    {
        public int CategoryId { set; get; }

        // các thông tin cơ bản về category
        // nhưng thông tin có thể nhiều ngôn ngữ thì chỉ lằm ở bảng này
        public string Name { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string LanguageId { set; get; }
        public string SeoAlias { set; get; }

        // thông tin không phụ thuộc vào ngôn ngữ
        public Category Category { get; set; }

        public Language Language { get; set; }
    }
}
