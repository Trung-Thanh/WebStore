using eShopSolution.Appication.Catalog.products.DataTransferObject;
using eShopSolution.Appication.Catalog.products.DataTransferObject.forPublic;
using eShopSolution.Appication.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Appication.Catalog.products
{
    public interface IPublicProductService
    {
        PageResult<ProductViewModel> GetAllByCategoryId(PlProductPagingRequest request); 
    }
}
