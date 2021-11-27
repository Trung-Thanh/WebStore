using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Catalog.product.forManager;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiEntegration
{
    public interface IProductApiClient
    {
        Task<PageResult<CMProductViewModel>> GetPagings(MngProductPagingRequest request);
        Task<bool> Create(ProductCreateRequest request);

        Task<CMProductViewModel> GetById(int id, string languageId);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<List<CMProductViewModel>> GetFeaturedProducts(string languageId, int take);
    }
}
