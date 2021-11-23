using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aShopSolution.AdminApp.Service
{
    public  interface IProductApiClient
    {
        Task<PageResult<CMProductViewModel>> GetPagings(MngProductPagingRequest request);

        Task<bool> Create(ProductCreateRequest request);
    }
}
