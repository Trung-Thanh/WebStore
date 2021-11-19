 using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using aShopSolution.AdminApp.Service;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace aShopSolution.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        //private readonly IConfiguration _configuration;

        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
            //_configuration = configuration;
        }
        
        // pageIndex is given by viewComponent
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var data = await _userApiClient.GetUsersPagings(request);
            return View(data.resultObj);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // remove token on session before sign out
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);
            if (result.isSuccessed)
                return RedirectToAction("Index");

            ModelState.AddModelError("", result.message);
            return View(request);
        }

        // update user
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetUserById(id);
            if (result.isSuccessed)
            {
                var userVM = result.resultObj;
                var updateUserRequest = new UserUpdateRequest()
                {
                    Dob = userVM.DoB,
                    Email = userVM.Email,
                    FirstName = userVM.FirstName,
                    LastName = userVM.LastName,
                    PhoneNumber = userVM.PhoneNumber,
                    id = userVM.Id
                };
                return View(updateUserRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            // show error of validation
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdateUser(request.id, request);

            if (result.isSuccessed)
            {
                return RedirectToAction("Index");
            }

            // so only model error = only bussiness error
            ModelState.AddModelError("", result.message);
            return View(request);
        }

        // user detail
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetUserById(id);
            return View(result.resultObj); 
        }

        //delete user
        [HttpGet]
        // id is given from detail, (edit link)
        public async Task<IActionResult> Delete(Guid id)
        {
            return View(
                new UserDeleteRequest()
                {
                    Id = id
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            // show error of validation
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Delete(request.Id);

            if (result.isSuccessed)
            {
                return RedirectToAction("Index");
            }

            // so only model error = only bussiness error
            ModelState.AddModelError("", result.message);
            return View(request);
        }
    }
}
