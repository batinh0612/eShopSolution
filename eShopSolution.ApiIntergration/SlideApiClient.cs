using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        public SlideApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
           : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        public async Task<List<SlideViewModel>> GetAll()
        {
            return await GetListAsync<SlideViewModel>("/api/slides");
        }
    }
}
