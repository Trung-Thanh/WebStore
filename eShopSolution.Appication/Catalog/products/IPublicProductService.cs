using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forPublic;
using eShopSolution.ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Appication.Catalog.products
{
    public interface IPublicProductService
    {
        Task<PageResult<CMProductViewModel>> GetAllByCategoryId(string languageId, PlProductPagingRequest request);
    }
}
