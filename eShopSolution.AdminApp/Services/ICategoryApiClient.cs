using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface ICategoryApiClient
    {
        /// <summary>
        /// Get alls
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId);
    }
}
