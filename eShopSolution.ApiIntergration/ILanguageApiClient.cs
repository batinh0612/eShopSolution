using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public interface ILanguageApiClient
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
