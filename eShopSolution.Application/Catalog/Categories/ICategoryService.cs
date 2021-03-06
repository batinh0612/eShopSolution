﻿using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all paging
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryViewModel>> GetAllPaging(string languageId);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CategoryViewModel> GetById(string languageId, int id);
    }
}
