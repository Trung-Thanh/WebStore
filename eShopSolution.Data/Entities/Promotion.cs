using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Promotion
    {
        // chính sách khuyến mại
        public int Id { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
        public bool ApplyForAll { set; get; }

        // 1 trong 2 hoặc giảm theo phần trăm, hay giảm theo khoảng tiền
        public int? DiscountPercent { set; get; }
        public decimal? DiscountAmount { set; get; }

        // chứa 1 loạt các mã - sản phấm được giảm
        public string ProductIds { set; get; }

        // chứa 1 loạt các mã - danh mục sản phấm được giảm
        public string ProductCategoryIds { set; get; }
        public Status Status { set; get; }
        public string Name { set; get; }
    }
}
