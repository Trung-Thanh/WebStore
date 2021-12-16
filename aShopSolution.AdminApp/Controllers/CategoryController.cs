using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aShopSolution.AdminApp.Models;
using eShopColution.Utilities.Constants;
using eShopSolution.ApiEntegration;
using eShopSolution.ViewModels.Catalog.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aShopSolution.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public CategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        void AssignLevel(List<CategoryViewModel> categories)
        {
            foreach (var category in categories)
            {
                if (category.ParentID == null)
                {
                    category.CategoryLv = 1;
                }
                else if (category.ParentID != null)
                {
                    var grandParentId = categories.Where(x => x.Id == category.ParentID).FirstOrDefault();
                    if (grandParentId.ParentID == null)
                    {
                        category.CategoryLv = 2;
                    }
                    else
                    {
                        category.CategoryLv = 3;
                    }
                }
            }
        }
        public async Task<IActionResult> Index(int? categoryId, int categoryLvFilter)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var categories = await _categoryApiClient.GetAll(currentLanguageId);

            AssignLevel(categories);

            if (categoryId != null)
            {
                categories.RemoveAll(x => x.ParentID != categoryId && x.CategoryLv == categoryLvFilter);
            }

            ViewBag.SelectedCategory = categoryId.HasValue ? categoryId : -1;

            if (TempData["result"] != null)
            {
                ViewBag.successMsg = TempData["result"];
            }

            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategory(CUCategoryRequest request)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApiClient.CreateCategory(request);


            if (result)
            {
                TempData["result"] = "Thêm mới danh mục thành công";
                //empData["addedProductId"] = result;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm danh mục không thành công");
            return View(request);
        }

        [HttpGet]
        public IActionResult CreateCategoryLv2()
        {
            return View(GetCreateRequestViewModel(2));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategoryLv2(CUCategoryRequest request)
        {

            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApiClient.CreateCategory(request);

            if (result)
            {
                TempData["result"] = "Thêm mới danh mục thành công";
                //empData["addedProductId"] = result;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm danh mục không thành công");
            return View(request);
        }

        [HttpGet]
        public IActionResult CreateCategoryLv3()
        {
            return View(GetCreateRequestViewModel(3));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategoryLv3(CUCategoryRequest request)
        {

            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApiClient.CreateCategory(request);

            if (result)
            {
                TempData["result"] = "Thêm mới danh mục thành công";
                //empData["addedProductId"] = result;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm danh mục không thành công");
            return View(request);
        }

        async Task<CreateSubCategoryViewModel> GetCreateRequestViewModel(int categoryLv)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var categories = await _categoryApiClient.GetAll(currentLanguageId);

            AssignLevel(categories);

            categories.RemoveAll(x => x.CategoryLv != categoryLv);

            CreateSubCategoryViewModel request = new CreateSubCategoryViewModel()
            {
                ListParents = categories,
            };

            return request;
        }

        // UPDATE
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var category = await _categoryApiClient.GetById(id, currentLanguageId);

            CUCategoryRequest uCategoryRequest = new CUCategoryRequest()
            {
                Name = category.Name,
                parentId = (int)category.ParentID,
                SeoAlias = category.SeoAlias,
                SeoDescription = category.SeoDescription,
                SeoTitle = category.SeoTitle
            };

            CreateSubCategoryViewModel request = new CreateSubCategoryViewModel();
            request.ucCategoryRequest = uCategoryRequest;

            var categories = await _categoryApiClient.GetAll(currentLanguageId);
            AssignLevel(categories);

            int thisCategoryLv = categories.FirstOrDefault(x=>x.Id==id).CategoryLv;

            if (thisCategoryLv > 1)
            {
                categories.RemoveAll(x => x.CategoryLv != (thisCategoryLv - 1));
                request.ListParents = categories;
            }
            else
            {
                request.ListParents = new List<CategoryViewModel>();
            }
            
            return View(request);
        }

        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _userProductApiClient.Update(request);
        //    if (result)
        //    {
        //        TempData["result"] = "cập nhật sản phẩm thành công";
        //        //TempData["addedProductId"] = request.id;
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", "cập nhật phẩm không thành công");
        //    return View(request);
        //}

    }
}
