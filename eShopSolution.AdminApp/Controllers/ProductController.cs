using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.Utilities.Constant;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, IConfiguration configuration, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var languageId = HttpContext.Session.GetString(SystemConstant.AppSettings.DefaultLanguageId);

            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId,
                CategoryId = categoryId
            };

            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.Success = TempData["result"];
            }

            var categories = await _categoryApiClient.GetAll(languageId);

            ViewBag.Categories = categories.Select(x => new SelectListItem() { 
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            var data = await _productApiClient.GetPaging(request);

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]//cho phep nhan kieu du lieu truyen len la form-data
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.Create(request);

            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");

            return View(request);
        }

        /// <summary>
        /// Role assign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AssignCategory(int id)
        {
            var categoryAssignRequest = await GetCategoryAssignRequest(id);

            return View(categoryAssignRequest);
        }

        /// <summary>
        /// Role assign
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AssignCategory(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.AssignCategory(request.Id, request);

            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);

            return View(roleAssignRequest);
        }

        /// <summary>
        /// GetRoleAssignRequest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstant.AppSettings.DefaultLanguageId);

            var productObj = await _productApiClient.GetById(id, languageId);

            var categories = await _categoryApiClient.GetAll(languageId);
            
            var categoryAssignRequest = new CategoryAssignRequest();

            foreach (var category in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectedItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = productObj.Categories.Contains(category.Name)
                });
            }
            return categoryAssignRequest;
        }
    }
}
