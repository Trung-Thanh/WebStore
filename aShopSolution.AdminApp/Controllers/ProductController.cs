using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aShopSolution.AdminApp.Service;
using eShopColution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.forManager;
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

            var data = await _userProductApiClient.GetPagings(request);
            ViewBag.keyword = keyword;

            var categories = await _categoryApiClient.GetAll(currentLanguageId);
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
            if (!ModelState.IsValid)
                return View();

            var result = await _userProductApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm không thành công");
            return View(request);
        }
    }
}
