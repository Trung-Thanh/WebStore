 using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }
 

        //thể hiện rằng một trường - một sản phẩm có một list nhưng loại mà nó thuộc về
        public List<ProductsInCategories> ProductsInCategories { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
        public List<Cart> carts { get; set; }
        public List<ProductTranslation> productTranslations { get; set; }
        public List<ProductImage> productImages { get; set; }
    }
}
