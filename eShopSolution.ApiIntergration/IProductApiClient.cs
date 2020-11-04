using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
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

        /// <summary>
        /// Get featured products
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<List<ProductViewModel>> GetFeaturedProducts(string languageId);

        /// <summary>
        /// Get latest products
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<List<ProductViewModel>> GetLatestProducts(string languageId, int take);
    }
}
