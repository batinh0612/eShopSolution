using eShopSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Utilities.Slides
{
    public interface ISlideService
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        Task<List<SlideViewModel>> GetAll();
    }
}
