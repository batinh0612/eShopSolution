using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductService
    {
        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<int> Create(ProductCreateRequest request);

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<int> Update(ProductUpdateRequest request);

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<int> Delete(int productId);

        /// <summary>
        /// Update Price Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="newPrice"></param>
        /// <returns></returns>
        Task<bool> UpdatePrice(int productId, decimal newPrice);

        /// <summary>
        /// Update Stock Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="addedQuantity"></param>
        /// <returns></returns>
        Task<bool> UpdateStock(int productId, int addedQuantity);

        /// <summary>
        /// Add View Count
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task AddViewCount(int productId);

        /// <summary>
        /// Get All Paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        /// <summary>
        /// Add images
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        Task<int> AddImages(int productId, IFormFile files);

        /// <summary>
        /// Remove images
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        Task<int> RemoveImages(int imageId);

        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="Caption"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        Task<int> UpdateImage(int imageId, string caption, bool isDefault);

        /// <summary>
        /// Get list image
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}
