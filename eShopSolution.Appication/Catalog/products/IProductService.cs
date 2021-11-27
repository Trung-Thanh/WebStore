using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Catalog.forPublic;
using eShopSolution.ViewModels.Catalog.product.forManager;
using eShopSolution.ViewModels.Catalog.ProductImage;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Appication.Catalog.products
{
    public interface IProductService
    {
        // public
        Task<PageResult<CMProductViewModel>> GetAllByCategoryId(string languageId, PlProductPagingRequest request);

        // return product id 
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<bool> UpdatePrice(int prodcutId, decimal newPrice);

        Task<bool> UpdateStock(int prodcutId, int addedQuantity);

        Task<CMProductViewModel> GetById(int productId, string languageId);

        Task AddViewCount(int productId);

        Task<int> Delete(int productID);

        // this method to get a page of product
        Task<PageResult<CMProductViewModel>> GetAllPaging(MngProductPagingRequest request);

        // Work with image:
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);
        Task<CMProductImageViewModel> GetImageById(int imageId);
        Task<List<CMProductImageViewModel>> GetListImages(int productId);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<List<CMProductViewModel>> GetFeatureProducts(string languageId, int take);
    }
}
