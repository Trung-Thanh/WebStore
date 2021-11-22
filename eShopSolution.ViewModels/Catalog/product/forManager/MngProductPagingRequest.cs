using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.forManager
{
    public class MngProductPagingRequest : PagingRequestBase
    {
        public string keyWord { get; set; }
        //public List<int> listCategoryIds { get; set; }

        public string LanguageId { get; set; }
    }
}
