using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _manageProductService;

        public ProductsController(IProductService manageProductService)
        {
            _manageProductService = manageProductService;
        } 

        /// <summary>
        /// Get all paging
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //http://localhost:port/products/pageIndex=1&pageSize=10&categoryId=
        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId ,[FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _manageProductService.GetAllByCategoryId(languageId ,request);
            return Ok(products);
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //http://localhost:port/product/
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
            if (product == null)
            {
                return BadRequest("Cannot find product");
            }

            return Ok(product);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(request);
            if (productId == 0)
            {
                return BadRequest();
            }

            var product = await _manageProductService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _manageProductService.Delete(productId);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Update price
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPrice"></param>
        /// <returns></returns>
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Create image
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageId = await _manageProductService.AddImage(productId, request);

            if (imageId == 0)
                return BadRequest();

            var image = await _manageProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpGet("{productId}/image/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _manageProductService.GetImageById(imageId);

            if (image == null)
                return BadRequest("Cannot find product image");

            return Ok(image);
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm]ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _manageProductService.UpdateImage(imageId, request);

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPost("{productId}/image/imageId")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            var result = await _manageProductService.RemoveImage(imageId);

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpGet("images/{productId}")]
        public async Task<IActionResult> ListImage(int productId)
        {
            var listImages = await _manageProductService.GetListImages(productId);
            return Ok(listImages);
        }
    }
}
