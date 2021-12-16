using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.product.forManager
{
    public class CategoryAssignRequest
    {
        // id of the product
        public int Id { get; set; }
        public List<SelectedItem> Categories { get; set; } = new List<SelectedItem>();
    }
}
