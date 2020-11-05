using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public interface ICategoryApiClient
    {
        /// <summary>
        /// Get alls
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<List<CategoryViewModel>> GetAll(string languageId);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CategoryViewModel> GetById(string languageId, int id);
    }
}
