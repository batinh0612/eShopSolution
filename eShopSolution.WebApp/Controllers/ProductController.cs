using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ApiIntergration;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            return View();
        }

        public async Task<IActionResult> Category(int id, string culture, int page = 1)
        {
            var products = await _productApiClient.GetPaging(new GetManageProductPagingRequest() { 
                CategoryId = id,
                LanguageId = culture,
                PageIndex = page,
                PageSize = 10
            });

            return View(new ProductCategoryViewModel() { 
                Category = await _categoryApiClient.GetById(culture, id),
                Products = products
            });
        }
    }
}
