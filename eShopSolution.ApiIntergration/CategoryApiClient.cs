using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
           : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            return await GetAsync<List<CategoryViewModel>>($"/api/categories?languageId={languageId}");
        }
    }
}
