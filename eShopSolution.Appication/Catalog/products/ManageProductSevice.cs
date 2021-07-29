using eShopColution.Utilities;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.Appication.Catalog.products
{
    class ManageProductSevice : IManageProductSevice
    {
        private readonly EShopDBContext _context;
        public ManageProductSevice(EShopDBContext context)
        {
            this._context = context;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product() {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                productTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias = request.SeoAlias,
                        LanguageId = request.LanguageId
                    }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productID)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product is null)
                throw new eShopSolutionExcreption($"can not find a product with id : {productID}");
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(MngProductPagingRequest request)
        {
            // 1.join 4 product, productInCategory, Category, productTranslation
            // note: request include keyword to match name of product (pt), 
            // and categoryId to filter base on categoryid
            var query = from p in _context.Products 
                        join pt in _context.productTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductsInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            // filter
            if (!string.IsNullOrEmpty(request.keyWord))
            {
                query = query.Where(x => x.pt.Name.Contains(request.keyWord.Trim()));
            }

            if(request.listCategoryIds.Count > 0)
            {
                query = query.Where(x => request.listCategoryIds.Contains(x.pic.CategoryId));
            }

            // pagging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.pageSize - 1) * request.pageSize).Take(request.pageSize)
                .Select(x => new ProductViewModel()
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
            var pageResult = new PageResult<ProductViewModel>()
            {
                totalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            // khong dung de sua thong tin, chi dung de kiem tra xem co tim thay khong
            var product = await _context.Products.FindAsync(request.id);
            var productTranslation = await _context.productTranslations.FirstOrDefaultAsync(x => x.ProductId == request.id && x.LanguageId == request.LanguageId);
            if(product is null || productTranslation is null)
            {
                throw new eShopSolutionExcreption($"can not find a product with id = {request.id}");
            }
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int prodcutId, decimal newPrice)
        {
            // chua nhat thien san pham do phai co trong translation roi
            var product = await _context.Products.FindAsync(prodcutId);
            if(product is null)
            {
                throw new eShopSolutionExcreption($"can not find a product with id = {prodcutId}");
            }
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int prodcutId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(prodcutId);
            if (product is null)
            {
                throw new eShopSolutionExcreption($"can not find a product with id = {prodcutId}");
            }
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
