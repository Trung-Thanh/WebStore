using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forPublic;
using eShopSolution.ViewModels.Common;
using System.Threading.Tasks;

namespace eShopSolution.Appication.Catalog.products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(PlProductPagingRequest request); 
    }
}
