using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopColution.Utilities.Constants;
using eShopSolution.ApiEntegration;
using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Catalog.product.forManager;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace aShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _userProductApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        //private readonly IConfiguration _configuration;

        public ProductController(IProductApiClient userProductApiClient, ICategoryApiClient categoryApiClient)
        {
            _userProductApiClient = userProductApiClient;
            _categoryApiClient = categoryApiClient;
            //_configuration = configuration;
        }

        // pageIndex is given by viewComponent
        public async Task<IActionResult> Index(int? categoryId, string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var request = new MngProductPagingRequest()
            {
                keyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = currentLanguageId,
                CategoryId = categoryId
            };

            // get one page
            var data = await _userProductApiClient.GetAllPaging_DontContainImg(request);

            // keep keyword dont disapear
            ViewBag.keyword = keyword;

            var categories = await _categoryApiClient.GetAll(currentLanguageId);

            // show drop down category on view
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && x.Id == categoryId
            });

            // the first time redirect
            if (TempData["result"] != null)
            {
                ViewBag.successMsg = TempData["result"];
                if(TempData["addedProductId"]!=null)
                ViewBag.AddedProductId = TempData["addedProductId"];
            }
            return View(data);
        }

        // create product

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            
            request.LanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            if (!ModelState.IsValid)
                return View();

            var result = await _userProductApiClient.Create(request);

            if (result>0)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                TempData["addedProductId"] = result;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm không thành công");
            return View(request);
        }

        // assign categories for product
        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var categoryAssignRequest = await GetCategoryAssignRequest(id);
            return View(categoryAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            // show error of validation
            if (!ModelState.IsValid)
                return View();

            // this id maybe have to have same name with id on URL of API server 
            var result = await _userProductApiClient.CategoryAssign(request.Id, request);

            if (result.isSuccessed)
            {
                TempData["result"] = "Gán danh mục thành công";
                return RedirectToAction("Index");
            }

            // so only model error = only bussiness error
            ModelState.AddModelError("", result.message);
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);
            return View(roleAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);


            var getProductResult = await _userProductApiClient.GetById(id, currentLanguageId);
            var getCategoryResult = await _categoryApiClient.GetAll(currentLanguageId);
            var categoryAssignRequest = new CategoryAssignRequest();

            foreach (var category in getCategoryResult)
            {
                categoryAssignRequest.Categories.Add(new SelectedItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = getProductResult.Categories.Contains(category.Name)
                });
            }

            return categoryAssignRequest;
        }

        // update
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var product = await _userProductApiClient.GetById(id, currentLanguageId);

            var updateProductRequest = new ProductUpdateRequest()
            {
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                IsFeature = product.IsFeature,
                id = product.Id,
                LanguageId = product.LanguageId
            };

            return View(updateProductRequest);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request, string AddOrReplace)
        {            
            if (!ModelState.IsValid)
                return View();

            if (AddOrReplace != null)
            {
                if (AddOrReplace.Equals("Add"))
                {
                    request.IsReplace = false;
                }
                else
                {
                    request.IsReplace = true;
                }
            }
            
             var result = await _userProductApiClient.Update(request);
            if (result)
            {
                TempData["result"] = "cập nhật sản phẩm thành công";
                TempData["addedProductId"] = request.id;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "cập nhật phẩm không thành công");
            return View(request);
        }

        // delete

        [HttpGet]
        // id form delete link on index page
        public IActionResult Delete(int id)
        {
            return View(new DeleteProductRequest { Id = id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteProductRequest request)
        {
            // show error of validation
            if (!ModelState.IsValid)
                return View();

            var result = await _userProductApiClient.Delete(request.Id);

            if (result)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            // so only model error = only bussiness error
            ModelState.AddModelError("", "xóa sản phẩm không thành công");
            return View(request);
        }

        // detail
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var product = await _userProductApiClient.GetById(id, currentLanguageId);

            var productViewModel = new CMProductViewModel()
            {
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                IsFeature = product.IsFeature,
                Id = product.Id,
                ThumbnailImage = product.ThumbnailImage,
                LittleFingernails = product.LittleFingernails,
                DateCreated = product.DateCreated,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                Stock = product.Stock
            };

            return View(productViewModel);
        }
    }
}
