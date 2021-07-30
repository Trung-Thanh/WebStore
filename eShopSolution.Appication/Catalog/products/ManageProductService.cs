using eShopColution.Utilities.Exceptions;
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
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolution.Appication.Common;
using eShopSolution.ViewModels.Catalog.product;

namespace eShopSolution.Appication.Catalog.products
{
    class ManageProductService : IManageProductSevice
    {
        private readonly EShopDBContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDBContext context, IStorageService storageService)
        {
            this._context = context;
            _storageService = storageService;
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
            // save image
            if(request.ThumbnailImage != null)
            {
                product.productImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        caption = "Thmbnail image",
                        dateCreate = DateTime.Now,
                        fileSize = request.ThumbnailImage.Length,
                        imagePath = await this.SaveFile(request.ThumbnailImage),
                        isDefault = true,
                        sortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productID)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product is null)
                throw new eShopSolutionExcreption($"can not find a product with id : {productID}");

            var images = _context.productImages.Where(x => x.productId == productID);
            foreach(var image in images)
            {
                await _storageService.DeleteFileAsync(image.imagePath);
            }

            _context.Products.Remove(product);  
            return await _context.SaveChangesAsync();
        }

        public async Task<PageResult<CMroductViewModel>> GetAllPaging(MngProductPagingRequest request)
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
                .Select(x => new CMroductViewModel()
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
            var pageResult = new PageResult<CMroductViewModel>()
            {
                totalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        // TO
        public async Task<int> AddImages(int productId, List<FormFile> files)
        {
            if (files.Count > 0 && files != null)
            {
                foreach (var file in files)
                {
                    await _context.productImages.AddAsync(
                        new ProductImage()
                        {
                            productId = productId,
                            dateCreate = DateTime.Now,
                            fileSize = file.Length,
                            imagePath = await this.SaveFile(file),
                        }
                    );
                }
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<List<CMProductImageViewModel>> getImageList(int productId)
        {
            var ImageList = _context.productImages.Where(x => x.productId == productId);
            var data = ImageList.Select(y => new CMProductImageViewModel()
            {
                filePath = y.imagePath,
                fileSize = y.fileSize,
                id = y.id,
                isDefault = y.isDefault
            }).ToListAsync();
            List<CMProductImageViewModel> ls = new List<CMProductImageViewModel>();
            ls = await data;
            return ls;
        }

        public async Task<int> RemoveImages(int imageId)
        {
            var imageTodel = await _context.productImages.FindAsync(imageId);
            if (imageTodel is null)
                throw new eShopSolutionExcreption($"can not find a product with id : {imageId}");
            _context.productImages.Remove(imageTodel);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            var productImage = _context.productImages.FirstOrDefault(x => x.id == imageId);
            if (productImage is null)
                throw new eShopSolutionExcreption($"can not find a product with id : {imageId}");
            productImage.caption = caption;
            productImage.isDefault = isDefault;
            return await _context.SaveChangesAsync();
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

            // save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage =await _context.productImages.FirstOrDefaultAsync(i => i.isDefault == true && i.productId == request.id);
                if(thumbnailImage != null)
                {
                    thumbnailImage.fileSize = request.ThumbnailImage.Length;
                    thumbnailImage.imagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.productImages.Update(thumbnailImage);
                }
            }

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

        private async Task<string> SaveFile(IFormFile file)
        {
            // lay ra ten file goc
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            // tao mot file name ngau nhien
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            // luu lai file voi ten la filename
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);

            // tra ve filename
            return fileName;
        }
    }
}
