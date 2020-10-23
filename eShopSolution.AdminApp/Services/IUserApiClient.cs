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
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        /// <summary>
        /// Get users paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request);

        /// <summary>
        /// Register request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<bool>> RegisterUser(RegisterRequest request);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<bool>> DeleteUser(Guid id);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<UserViewModel>> GetById(Guid id);
    }
}
