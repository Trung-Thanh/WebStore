using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Appication.Catalog.products
{
    public interface IManageProductSevice
    {
        // tra ve ma san phan vua tao
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<bool> UpdatePrice(int prodcutId, decimal newPrice);

        Task<bool> UpdateStock(int prodcutId, int addedQuantity);

        Task AddViewCount(int productId);

        Task<int> Delete(int productID);

        Task<PageResult<ProductViewModel>> GetAllPaging(MngProductPagingRequest request);  
    }
}
