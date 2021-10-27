using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Appication.Catalog.products;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Catalog.forPublic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // decalare iterface work with product public service
        private readonly IPublicProductService _publicProductSeverice;

        // decalare iterface work with product private service
        private readonly IManageProductSevice _manageProductService;

        // bellow method will be call form startup class
        public ProductController(IPublicProductService publicProductSeverice, IManageProductSevice manageProductSevice)
        {
            _publicProductSeverice = publicProductSeverice;
            _manageProductService = manageProductSevice;
        }
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId) 
        {
            var product = await _publicProductSeverice.GetAll(languageId);
            return Ok(product);
        }

        [HttpGet("GetProductPagingByCategoryId")]
        public async Task<IActionResult> Get([FromQuery] PlProductPagingRequest request)
        {
            var product = await _publicProductSeverice.GetAllByCategoryId(request);
            return Ok(product);
        }

        // Manage service

        //http://localhost:port/product/id
        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(int id, string languageId)
        {
            var product = await _manageProductService.GetById(id, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var productId = await _manageProductService.Create(request);
            if (productId == 0)
                return BadRequest();

            // get the product creared, so we need get by id method
            var product = await _manageProductService.GetById(productId, request.LanguageId);

            // name of getbyid is the abover method defined of manager service
            // to view what product was created, use this method
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _manageProductService.Delete(id);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPut("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(id, newPrice);
            if (isSuccessful)
                return Ok();

            return BadRequest();
        }
    }
}
