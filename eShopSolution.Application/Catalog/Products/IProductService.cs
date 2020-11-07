using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
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
        /// Get by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<ProductViewModel> GetById(int productId, string languageId);

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
        /// Add image
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        /// <summary>
        /// Remove images
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<int> RemoveImage(int imageId);

        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="imageId"></param>
        /// <param name="productImageViewModel"></param>
        /// <returns></returns>
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        /// <summary>
        /// Get list image
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<List<ProductImageViewModel>> GetListImages(int productId);

        /// <summary>
        /// Get image by id
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        Task<ProductImageViewModel> GetImageById(int imageId);

        /// <summary>
        /// Get all by categoryid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);

        /// <summary>
        /// Assign category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<bool>> AssignCategory(int id, CategoryAssignRequest request);

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
