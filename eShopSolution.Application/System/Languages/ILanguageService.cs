using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Languages
{
    public interface ILanguageService
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
