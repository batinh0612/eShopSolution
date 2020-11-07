using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Utilities.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoriesController(ICategoryService categoryService, IHttpContextAccessor httpContextAccessor)
        {
            _categoryService = categoryService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string languageId)
        {

            var categories = await _categoryService.GetAllPaging(languageId);

            return Ok(categories);
        }

        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(string languageId, int id)
        {
            var category = await _categoryService.GetById(languageId, id);
            return Ok(category);
        }
    }
}
