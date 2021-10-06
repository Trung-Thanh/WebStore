using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.product
{
    public class CMProductImageViewModel
    {
        public int id { get; set; }
        public string filePath { get; set; }
        public bool isDefault { get; set; }
        public long fileSize { get; set; }
    }
}
