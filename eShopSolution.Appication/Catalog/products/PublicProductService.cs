using eShopSolution.Data.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forPublic;

namespace eShopSolution.Appication.Catalog.products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDBContext _context;
        public PublicProductService(EShopDBContext context)
        {
            this._context = context;
        }

        public async Task<PageResult<CMProductViewModel>> GetAllByCategoryId(string languageId, PlProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.productTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductsInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            // filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }

            // pagging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                .Select(x => new CMProductViewModel()
                {
                    Id = x.p.Id,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    Name = x.pt.Name,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            // select and projection
            var pageResult = new PageResult<CMProductViewModel>()
            {
                totalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }
    }
}
