using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        //Khoi tao 1 lan
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.Success = TempData["result"];
            }

            var data = await _userApiClient.GetUsersPaging(request);

            return View(data.ResultObj);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm mới người dùng thành công";
                return RedirectToAction("Index", "User");
            }
                

            ModelState.AddModelError("", result.Message);

            return View(request);
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);

            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật người dùng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);

            return View(request);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var user = new UserDeleteRequest()
            {
                Id = id
            };

            return View(user);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.DeleteUser(request.Id);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index", "User");
            }
                
            return RedirectToAction("Error", "Home");
        }

        /// <summary>
        /// Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id) 
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.GetById(id);

            if (result.IsSuccessed)
                return View(result.ResultObj);

            return View(result.ResultObj);
        }

        /// <summary>
        /// Role assign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);

            return View(roleAssignRequest);
        }

        /// <summary>
        /// Role assign
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RoleAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);

            var roleAssignRequest = await GetRoleAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        /// <summary>
        /// GetRoleAssignRequest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectedItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }
    }
}
