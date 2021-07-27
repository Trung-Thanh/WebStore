using eShopSolution.Appication.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Appication.Catalog.products.DataTransferObject.forPublic
{
    public class PlProductPagingRequest : PagingRequestBase
    {
        public int CategoryId { get; set; }
    }
}
