using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly EShopDbContext DbContext;

        public SlideService(EShopDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        public async Task<List<SlideViewModel>> GetAll()
        {
            var slides = await DbContext.Slides.Select(x => new SlideViewModel() { 
                Id = x.Id,
                Description = x.Description,
                Image = x.Image,
                Name = x.Name,
                SortOrder = x.SortOrder,
                Status = x.Status,
                Url = x.Url
            }).OrderBy(x => x.SortOrder).ToListAsync();

            return slides;
        }
    }
}
