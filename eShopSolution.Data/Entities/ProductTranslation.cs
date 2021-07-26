using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductTranslation
    {
        // 1 ma product co the có nhiefu record nên k thể làm khóa chính
        public int ProductId { set; get; }

        // thông tin sản phẩm nhưng nhiều ngồn ngữ, mỗi trường được viết bởi 1 ngôn ngữ
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        // mã ngôn ngữ của, 1 record nào đó của bảng ngôn ngữ
        public string LanguageId { set; get; }

        // nhiền hơn về thông tin sản phẩm, số lượng, giá ... (không bị ảnh hưởng bởi ngôn ngữ)
        public Product Product { get; set; }

        // class ngôn ngữ
        public Language Language { get; set; }
    }
}
