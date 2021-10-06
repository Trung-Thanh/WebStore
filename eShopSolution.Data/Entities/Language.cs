using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Language
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        // danh sách các sản phẩm được dịch theo ngôn ngữ đó, mà bảng ProductTranslation lại chưa thông tin đầy đủ của sản phẩm luôn
        //public List<ProductTranslation> ProductTranslations { get; set; }

        // Tương tự với list category
        public List<CategoryTranslation> CategoryTranslations { get; set; }
        public List<ProductTranslation> productTranslations { get; set; }
    }
}
