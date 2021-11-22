using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aShopSolution.AdminApp.Service;
using eShopColution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.forManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _userProductApiClient;
        //private readonly IConfiguration _configuration;

        public ProductController(IProductApiClient userProductApiClient)
        {
            _userProductApiClient = userProductApiClient;
            //_configuration = configuration;
        }

        // pageIndex is given by viewComponent
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var currentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new MngProductPagingRequest()
            {
                keyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = currentLanguageId
            };

            var data = await _userProductApiClient.GetPagings(request);
            ViewBag.keyword = keyword;

            // the first time redirect
            if (TempData["result"] != null)
            {
                ViewBag.successMsg = TempData["result"];
            }
            return View(data);
        }

        // create product
        

    }
}
