using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;

        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all paging
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryViewModel>> GetAllPaging(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            var listCategories =  await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParrentId = x.c.ParentId
            }).ToListAsync();

            return listCategories;
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CategoryViewModel> GetById(string languageId, int id)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId && c.Id == id
                        select new { c, ct };
            
            return await query.Select(x => new CategoryViewModel()
            { 
                Id = x.c.Id,
                Name = x.ct.Name,
                ParrentId = x.c.ParentId,
                Description = x.ct.SeoDescription
            }).SingleOrDefaultAsync();
        }
    }
}
