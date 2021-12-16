using eShopSolution.ViewModels.Catalog.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiEntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);

        Task<CategoryViewModel> GetById(int id, string languageId);

        Task<bool> CreateCategory(CUCategoryRequest request);

    }
}
