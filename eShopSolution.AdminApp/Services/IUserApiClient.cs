using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<string> Authenticate(LoginRequest request);

        /// <summary>
        /// Get users paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);
    }
}
