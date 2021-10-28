using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.ProductImage
{
    public class CMProductImageViewModel
    {
        public int id { get; set; }
        public int productId { get; set; }
        public string imagePath { get; set; }
        public string caption { get; set; }
        public bool isDefault { get; set; }
        public DateTime dateCreate { get; set; }
        public int sortOrder { get; set; }
        public long fileSize { get; set; }
    }
}
