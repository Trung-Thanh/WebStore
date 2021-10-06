using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Appication.Catalog.products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductSeverice;
        public ProductController(IPublicProductService publicProductSeverice)
        {
            _publicProductSeverice = publicProductSeverice;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _publicProductSeverice.GetAll();
            return Ok(product);
        }
    }
}
