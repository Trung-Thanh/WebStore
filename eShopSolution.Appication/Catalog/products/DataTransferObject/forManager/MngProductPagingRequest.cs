using eShopSolution.Appication.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Appication.Catalog.products.DataTransferObject.forManager
{
    public class MngProductPagingRequest : PagingRequestBase
    {
        public string keyWord { get; set; }
        public List<int> listCategoryIds { get; set; }
    }
}
