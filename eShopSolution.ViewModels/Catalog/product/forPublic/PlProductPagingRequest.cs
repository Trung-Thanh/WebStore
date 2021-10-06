using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.forPublic
{
    public class PlProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
