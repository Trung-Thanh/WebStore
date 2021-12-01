using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ApiEntegration;
using eShopSolution.ViewModels.Catalog.forManager;
using Microsoft.AspNetCore.Mvc;
using MyWebStore.Models;
using eShopColution.Utilities.Constants;
using static eShopColution.Utilities.Constants.SystemConstants;

namespace MyWebStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IProductApiClient _productApiClient;

        public ProductController(ICategoryApiClient categoryApiClient, IProductApiClient productApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _productApiClient = productApiClient;
        }
        // we added endpoint url to start up so we can get parameter at those method
        // culture, id get form endpoint url
        public async Task<IActionResult> Category(int id, string culture, int pageIndex=1)
        {
            var products = await _productApiClient.GetPagings(new MngProductPagingRequest() { 
                CategoryId = id,
                PageIndex = pageIndex,
                LanguageId = culture,
                PageSize = ProductSettings.numberOfProductsInCategory,
            });

            
            return View(new ProductsInCategoryViewModel() { 
                Category = await _categoryApiClient.GetById(id, culture),
                Products = products
            });
        }

        // culture, id get form endpoint url
        public async Task<IActionResult> Detail(int id, string culture)
        {
            var product = await _productApiClient.GetById(id, culture);
            return View(new ProductDetailViewModel()
            {
                Product = product,
                Category = await _categoryApiClient.GetById(1, culture)
            });

        }
    }
}
