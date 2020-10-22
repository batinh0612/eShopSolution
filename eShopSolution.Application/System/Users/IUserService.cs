using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> Authencate(LoginRequest request);

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> Register(RegisterRequest request);

        /// <summary>
        /// Get users paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);
    }
}
