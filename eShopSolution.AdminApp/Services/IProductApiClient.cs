using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IProductApiClient
    {
        /// <summary>
        /// Get paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResult<ProductViewModel>> GetPaging(GetManageProductPagingRequest request);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> Create(ProductCreateRequest request);

        /// <summary>
        /// Assign category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<bool>> AssignCategory(int id, CategoryAssignRequest request);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<ProductViewModel> GetById(int id, string languageId);
    }
}
