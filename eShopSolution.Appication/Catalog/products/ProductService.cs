using eShopColution.Utilities.Exceptions;
using eShopColution.Utilities.Constants;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
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
using eShopSolution.ViewModels.Catalog.ProductImage;
using eShopSolution.ViewModels.Catalog.forPublic;
using eShopSolution.ViewModels.Catalog.product.forManager;
using static eShopColution.Utilities.Constants.SystemConstants;

namespace eShopSolution.Appication.Catalog.products
{
    public class ProductService : IProductService
    {
        private readonly EShopDBContext _context;
        private readonly IStorageService _storageService;

        public ProductService(EShopDBContext context, IStorageService storageService)
        {
            // this constructor with be call someware.
            // to get dbcontext from argument
            this._context = context;

            // class that implement IStorageService
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
            var allLanguages = _context.Languages;
            var translationsForAllLanguage = new List<ProductTranslation>();
            
            foreach(var language in allLanguages)
            {
                if(language.Id == request.LanguageId)
                {
                    translationsForAllLanguage.Add(
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
                    );
                }
                else
                {
                    translationsForAllLanguage.Add(
                        new ProductTranslation()
                        {
                            Name = BussinessConstant.NA,
                            Description = BussinessConstant.NA,
                            Details = BussinessConstant.NA,
                            SeoDescription = BussinessConstant.NA,
                            SeoTitle = BussinessConstant.NA,
                            SeoAlias = BussinessConstant.NA,
                            LanguageId = language.Id
                        }
                    );
                }
            }

            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                productTranslations = translationsForAllLanguage,
                IsFeature = request.IsFeature
            };

            // save image
            if (request.ThumbnailImage != null || request.LittleFingernails != null)
            {
                product.productImages = new List<ProductImage>();

                if(request.ThumbnailImage != null)
                {
                    product.productImages.Add(
                        new ProductImage()
                        {
                            caption = "Thmbnail image",
                            dateCreate = DateTime.Now,
                            fileSize = request.ThumbnailImage.Length,
                            imagePath = await this.SaveFile(request.ThumbnailImage),
                            isDefault = true,
                            sortOrder = 1
                        });                  
                }
                if (request.LittleFingernails != null)
                {
                    foreach(var i in request.LittleFingernails)
                    {
                        product.productImages.Add(
                        new ProductImage()
                        {
                            caption = "LittleFingernails",
                            dateCreate = DateTime.Now,
                            fileSize = i.Length,
                            imagePath = await this.SaveFile(i),
                            isDefault = false,
                            sortOrder = 2
                        });
                    }    
                };
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productID)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product is null)
                throw new eShopSolutionExcreption($"can not find a product with id : {productID}");

            // deleta all image and data about the product
            var images = _context.productImages.Where(x => x.productId == productID);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.imagePath);
            }

            // có thể là xóa q ràng buộc luôn
            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }


        public async Task<PageResult<CMProductViewModel>> GetAllPaging_DontContainImg(MngProductPagingRequest request)
        {

            // 1.join 4: product, productInCategory, Category, productTranslation
            // note: request include keyword to match name of product (pt), 
            // and categoryId to filter base on categoryid
            var query = from p in _context.Products
                        join pt in _context.productTranslations on p.Id equals pt.ProductId
                        //into ppt
                        //from pt in ppt.DefaultIfEmpty()

                        join pic in _context.ProductsInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()

                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()

                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic};

            // filter
            // filter by keywork form admin
            if (!string.IsNullOrEmpty(request.keyWord))
            {
                query = query.Where(x => x.pt.Name.Contains(request.keyWord.Trim()));
            }

            // filter by al list of category id
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                // this query so strong
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }

            // pagging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
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
                TotalRecord = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return pageResult;
        }

        // this method bellow return all infor about a product
        public async Task<PageResult<CMProductViewModel>> GetAllPaging(MngProductPagingRequest request)
        {

            // 1.join 4: product, productInCategory, Category, productTranslation
            // note: request include keyword to match name of product (pt), 
            // and categoryId to filter base on categoryid
            var query = from p in _context.Products
                        join pt in _context.productTranslations on p.Id equals pt.ProductId 
                        //into ppt
                        //from pt in ppt.DefaultIfEmpty()

                        join pic in _context.ProductsInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()

                        join pi in _context.productImages on p.Id equals pi.productId into ppi
                        from pi in ppi.DefaultIfEmpty()

                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()

                        where pt.LanguageId == request.LanguageId && pi.isDefault == true
                        select new { p, pt, pic, pi };

            // filter
            // filter by keywork form admin
            if (!string.IsNullOrEmpty(request.keyWord))
            {
                query = query.Where(x => x.pt.Name.Contains(request.keyWord.Trim()));
            }

            // filter by al list of category id
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                // this query so strong
                query = query.Where(x => x.pic.CategoryId ==  request.CategoryId);
            }

            // pagging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
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
                    ViewCount = x.p.ViewCount,

                    ThumbnailImage = x.pi.imagePath

                }).ToListAsync();

            // select and projection
            var pageResult = new PageResult<CMProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return pageResult;
        }
        public async Task<int> Update(ProductUpdateRequest request)
        {
            // khong dung de sua thong tin, chi dung de kiem tra xem co tim thay khong
            var product = await _context.Products.FindAsync(request.id);

            // kiem tra xem có truong ngon ngu muon sua khong - tuc la kiem tra xem có áo đó ở ngôn ngữ đó k
            var productTranslation = await _context.productTranslations.
                FirstOrDefaultAsync(x => x.ProductId == request.id && x.LanguageId == request.LanguageId);

            if (product is null || productTranslation is null)
            {
                throw new eShopSolutionExcreption($"can not find a product with id = {request.id}");
            }

            product.IsFeature = request.IsFeature;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;

            // save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.productImages.FirstOrDefaultAsync(i => i.isDefault == true && i.productId == request.id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.fileSize = request.ThumbnailImage.Length;
                    thumbnailImage.imagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.productImages.Update(thumbnailImage);
                }
                else
                {
                    var image = new ProductImage()
                    {
                        caption = "ThumbnailImage",
                        dateCreate = DateTime.Today,
                        fileSize = request.ThumbnailImage.Length,
                        imagePath = await this.SaveFile(request.ThumbnailImage),
                        isDefault = true,
                        productId = request.id,
                        sortOrder = 1
                    };
                    await _context.productImages.AddAsync(image);
                }
            }
            if (request.LittleFingernails != null)
            {
                if (request.IsReplace)
                {
                    var fingernailForDel = await _context.productImages.Where(x => x.isDefault == false && x.productId == request.id).ToListAsync();
                    _context.productImages.RemoveRange(fingernailForDel);
                }
                foreach (var i in request.LittleFingernails)
                {
                    product.productImages.Add(
                    new ProductImage()
                    {
                        caption = "LittleFingernails",
                        dateCreate = DateTime.Now,
                        fileSize = i.Length,
                        imagePath = await this.SaveFile(i),
                        isDefault = false,
                        sortOrder = 2
                    });
                }
            }

            var result = await _context.SaveChangesAsync();
            return result;
        }


        public async Task<bool> UpdatePrice(int prodcutId, decimal newPrice)
        {
            // chua nhat thien san pham do phai co trong translation roi
            var product = await _context.Products.FindAsync(prodcutId);
            if (product is null)
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

        // this Save file trong do co SaveFileAsync
        private async Task<string> SaveFile(IFormFile file)
        {
            // lấy tên file gốc từ IFormFile
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";


            // luu file tren server
            // tham so dau la luong tu IFormFile
            // tham so thu 2 la ten luu tren server
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }

        public async Task<CMProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);

            var thumbnailImage = await _context.productImages.Where(x => x.isDefault == true && x.productId == productId).FirstOrDefaultAsync();

            var littlFingers = await _context.productImages.Where(x => x.isDefault == false && x.productId == productId).Select(x => x.imagePath).ToListAsync();

            var productTranslation = await _context.productTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);

            var categories = await (from c in _context.Categories
                              join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                              join pic in _context.ProductsInCategories on c.Id equals pic.CategoryId
                              where pic.ProductId == productId && ct.LanguageId == languageId
                              select ct.Name).ToListAsync();

            var productViewModel = new CMProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation != null ? productTranslation.LanguageId : null,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories,
                ThumbnailImage = thumbnailImage != null ? thumbnailImage.imagePath : "empty",
                IsFeature = product.IsFeature,
                LittleFingernails = littlFingers
            };
            return productViewModel;
        }

        // Implement Image jobs
        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                caption = request.Caption,
                dateCreate = DateTime.Now,
                isDefault = request.IsDefault,
                productId = productId,
                sortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.imagePath = await this.SaveFile(request.ImageFile);
                productImage.fileSize = request.ImageFile.Length;
            }
            _context.productImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.id;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.productImages.FindAsync(imageId);
            if (productImage == null)
                throw new eShopSolutionExcreption($"Cannot find an image with id {imageId}");
            _context.productImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.productImages.FindAsync(imageId);
            if (productImage == null)
                throw new eShopSolutionExcreption($"Cannot find an image with id {imageId}");

            // customize
            productImage.caption = request.Caption;
            productImage.dateCreate = DateTime.Now;
            productImage.isDefault = request.IsDefault;
            productImage.sortOrder = request.SortOrder;

            if (request.ImageFile != null)
            {
                productImage.imagePath = await this.SaveFile(request.ImageFile);
                productImage.fileSize = request.ImageFile.Length;
            }
            _context.productImages.Update(productImage);
            await _context.SaveChangesAsync();
            return productImage.id;
        }

        // use to show Craeted image
        public async Task<CMProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.productImages.FindAsync(imageId);
            if (image == null)
                throw new eShopSolutionExcreption($"Cannot find an image with id {imageId}");

            var viewModel = new CMProductImageViewModel()
            {
                caption = image.caption,
                dateCreate = image.dateCreate,
                fileSize = image.fileSize,
                id = image.id,
                imagePath = image.imagePath,
                isDefault = image.isDefault,
                productId = image.productId,
                sortOrder = image.sortOrder
            };
            return viewModel;
        }

        public async Task<List<CMProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.productImages.Where(x => x.productId == productId)
                .Select(i => new CMProductImageViewModel()
                {
                    caption = i.caption,
                    dateCreate = i.dateCreate,
                    fileSize = i.fileSize,
                    id = i.id,
                    imagePath = i.imagePath,
                    isDefault = i.isDefault,
                    productId = i.productId,
                    sortOrder = i.sortOrder
                }).ToListAsync();
        }

        // public
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

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
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
                TotalRecord = totalRow,
                Items = data,
                PageIndex =request.PageIndex,
                PageSize = request.PageSize
            };
            return pageResult;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Sản phẩm không tồn tại");
            }

            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductsInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
                    && x.ProductId == id);

                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductsInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductsInCategories.AddAsync(new ProductsInCategories()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = id
                    });
                }
            }

            var result = await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<CMProductViewModel>> GetFeatureProducts(string languageId, int take)
        {
            var query = from p in _context.Products
                        join pt in _context.productTranslations on p.Id equals pt.ProductId

                        join pi in _context.productImages on p.Id equals pi.productId into ppi
                        from pi in ppi.DefaultIfEmpty()

                        join pic in _context.ProductsInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()

                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()

                        where pt.LanguageId == languageId && (pi == null || pi.isDefault == true)
                        && p.IsFeature == true
                        select new { p, pt, pic, pi };

            // pagging
            int totalRow = await query.CountAsync();

            if (take > totalRow)
                take = totalRow;

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
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
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.imagePath
                }).ToListAsync();

            return data;
        }

        public async Task<List<CMProductViewModel>> GetLatestProducts(string languageId, int take)
        {
            var query = from p in _context.Products
                        join pt in _context.productTranslations on p.Id equals pt.ProductId

                        join pi in _context.productImages on p.Id equals pi.productId into ppi
                        from pi in ppi.DefaultIfEmpty()

                        join pic in _context.ProductsInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()

                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()

                        where pt.LanguageId == languageId && (pi == null || pi.isDefault == true)
                        select new { p, pt, pic, pi };

            // pagging
            int totalRow = await query.CountAsync();

            if (take > totalRow)
                take = totalRow;

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
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
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.imagePath
                }).ToListAsync();

            return data;
        }
    }
}
