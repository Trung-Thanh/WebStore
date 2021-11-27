using System;
using eShopColution.Utilities.Constants;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ApiEntegration;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebStore.Models;
using static eShopColution.Utilities.Constants.SystemConstants;

namespace MyWebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger, ISharedCultureLocalizer loc,
            ISlideApiClient slideApiClient, IProductApiClient productApiClient)
        {
            _logger = logger;
            _loc = loc;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var slides = await _slideApiClient.GetAll();
            var culture = CultureInfo.CurrentCulture.Name;
            var featureProducts = await _productApiClient.GetFeaturedProducts(culture, ProductSettings.numberOfFeaturedProducts);
            var viewModel = new HomeViewModel()
            {
                Slides = slides,
                FeaturedProducts = featureProducts
            };
            
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
