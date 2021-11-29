using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Appication.Catalog.products;
using eShopSolution.ViewModels.Catalog.forManager;
using eShopSolution.ViewModels.Catalog.forPublic;
using eShopSolution.ViewModels.Catalog.product.forManager;
using eShopSolution.ViewModels.Catalog.ProductImage;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApi.Controllers
{
    //api/prodcuts
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;

        // bellow method will be call form startup class
        public ProductsController(IProductService manageProductSevice)
        {
            _ProductService = manageProductSevice;
        }

        // https://localhost:port/products?pageIndex=1&pageSize=10&CategoryId=

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging([FromQuery] MngProductPagingRequest request)
        {
            var product = await _ProductService.GetAllPaging(request);
            return Ok(product);
        }

        /*Manage service*/

        //http://localhost:port/product/id
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _ProductService.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var productId = await _ProductService.Create(request);
            if (productId == 0)
                return BadRequest();

            // get the product creared, so we need get by id method
            var product = await _ProductService.GetById(productId, request.LanguageId);

            // name of getbyid is the abover method defined of manager service
            // to view what product was created, use this method
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update(/*[FromRoute] int productId ,*/[FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //request.id = productId;
            var affectedResult = await _ProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _ProductService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        // just update a part of whole object use HTTP Patch
        [HttpPatch ("price/{ProductId}/{newPrice}")]
        [Authorize]
        public async Task<IActionResult> UpdatePrice(int ProductId, decimal newPrice)
        {
            var isSuccessful = await _ProductService.UpdatePrice(ProductId, newPrice);
            if (isSuccessful)
                return Ok();

            return BadRequest();
        }

        /*Image jobs*/
        [HttpPost("{productId}/images")]
        [Authorize]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _ProductService.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _ProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        // update image dont need product id
        [HttpPut("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        // remove image dont need product id
        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.RemoveImage(imageId);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _ProductService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find product");
            return Ok(image);
        }

        // role assign
        [HttpPut("{id}/categories")]
        [Authorize]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ProductService.CategoryAssign(id, request);
            if (!result.isSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // get feature products
        [HttpGet("featured/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedProducts(string languageId, int take)
        {
            var featuredProducts = await _ProductService.GetFeatureProducts(languageId, take);
            if (featuredProducts == null)
            {
                return BadRequest("Cannot find featured products");
            }
            return Ok(featuredProducts);
        }

        // get latest products
        [HttpGet("latest/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestProducts(string languageId, int take)
        {
            var latestProducts = await _ProductService.GetLatestProducts(languageId, take);
            if (latestProducts == null)
            {
                return BadRequest("Cannot find latest products");
            }
            return Ok(latestProducts);
        }
    }
}
