using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolution.Application.Common;
using eShopSolution.ViewModels.Catalog.ProductImages;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        /// <summary>
        /// Get all paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            //select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        into ppic from pic in ppic.DefaultIfEmpty()//left join
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        into picc from c in picc.DefaultIfEmpty()//left join
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic};
            //filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);

            //paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    CreatedDate = x.p.CreatedDate,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };

            return pagedResult;
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);

            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);

            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToListAsync();

            var productViewModel = new ProductViewModel()
            {
                Id = productId,
                CreatedDate = product.CreatedDate,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories
            };

            return productViewModel;
        }

        /// <summary>
        /// Add view count
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                CreatedDate = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };

            //Save image
            if (request.ThumnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        CreatedDate = DateTime.Now,
                        FileSize = request.ThumnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product:  { productId} ");

            var images = _context.ProductImages.Where(x => x.ProductId == productId);
            foreach (var item in images)
            {
                await _storageService.DeleteFileAsync(item.ImagePath);
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Add images
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                CreatedDate = DateTime.Now,
                IsDefault = request.IsDefault,
                SortOrder = request.SortOrder,
                ProductId = productId
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();

            return productImage.Id;
        }

        /// <summary>
        /// Remove images
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find product image with id {imageId}");
            await _storageService.DeleteFileAsync(productImage.ImagePath);
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="imageId"></param>
        /// <param name="caption"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find product image with id {imageId}");

            productImage.Caption = request.Caption;
            productImage.IsDefault = request.IsDefault;
            productImage.SortOrder = request.SortOrder;

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);

            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get list images
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            var productImage = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
            if (productImage == null)
                throw new EShopException($"Cannot find product with id {productId}");

            var lstProductImages = new List<ProductImageViewModel>();
            foreach (var item in productImage)
            {
                var productImageViewModel = new ProductImageViewModel()
                {
                    Id = item.Id,
                    ImagePath = item.ImagePath,
                    FileSize = item.FileSize,
                    IsDefault = item.IsDefault
                };

                lstProductImages.Add(productImageViewModel);
            }

            return lstProductImages;
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(n => n.ProductId == request.Id && n.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null) throw new EShopException($"Cannot find a product with id: { request.Id }");

            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;

            //Save file
            if (request.ThumnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            _context.ProductTranslations.Update(productTranslation);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update price product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="newPrice"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");

            product.Price = newPrice;
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Update stock product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="addedQuantity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Get image by id
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);

            if (image == null) throw new EShopException($"Cannot find product image with id {imageId}");

            var imageViewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                CreatedDate = image.CreatedDate,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };

            return imageViewModel;
        }

        /// <summary>
        /// Save file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        /// <summary>
        /// Remove file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task RemoveFile(string fileName)
        {
            await _storageService.DeleteFileAsync(fileName);
        }

        /// <summary>
        /// Get all by categoryid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            //select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            //filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
                query = query.Where(p => request.CategoryId == request.CategoryId);

            //paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    CreatedDate = x.p.CreatedDate,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagedResult;
        }

        /// <summary>
        /// Assign category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> AssignCategory(int id, CategoryAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return new ApiErrorResult<bool>("Sản phẩm không tồn tại");

            foreach (var category in request.Categories)
            {
                //find by productId and categoryId
                var productInCategory = await _context.ProductInCategories.FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id) && x.ProductId == id);
                if (productInCategory != null & category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if(productInCategory == null && category.Selected == true)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory() { 
                        CategoryId = int.Parse(category.Id),
                        ProductId = id
                    });
                }
            }

            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }

        /// <summary>
        /// Get featured product
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<List<ProductViewModel>> GetFeaturedProducts(string languageId)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        && p.IsFeatured == true
                        select new { p, pt, pic, pi };

            var data = await query.OrderByDescending(x => x.p.CreatedDate)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    CreatedDate = x.p.CreatedDate,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            return data;
        }

        /// <summary>
        /// Get latest products
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<List<ProductViewModel>> GetLatestProducts(string languageId, int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        select new { p, pt, pic, pi };

            var data = await query.OrderByDescending(x => x.p.CreatedDate).Take(take)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    CreatedDate = x.p.CreatedDate,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            return data;
        }
    }
}
