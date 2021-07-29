using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductImage
    {
        public int id { get; set; }
        public int productId { get; set; }
        public string imagePath { get; set; }
        public string caption { get; set; }
        public bool isDefault { get; set; }
        public DateTime dateCreate { get; set; }
        public int sortOrder { get; set; }
        public int fileSize { get; set; }
        public Product product { get; set; }
    }
}
