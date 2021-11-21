using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace aShopSolution.AdminApp.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor )
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            //use api from WebApi
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // post request to uri of WebApi
            var response = await client.PostAsync("/api/users/authenticate", httpContent);

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return  JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            //get tocken form api
            return  JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }

        public async Task<ApiResult<PageResult<UserViewModel>>> GetUsersPagings(GetUserPagingRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // add token to header of http client
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            // don't catch error beacause this fucntion err with be catch at gloabal exception
            var users = JsonConvert.DeserializeObject<ApiSuccessResult<PageResult<UserViewModel>>>(body);

            return users;
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // needless to authenticate the tocken

            var json = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/users", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        // update
        public async Task<ApiResult<UserViewModel>> GetUserById(Guid id)
        {

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // add token to header of http client
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/users/{id}");

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiErrorResult<UserViewModel>>(body);
            }

            // don't catch error beacause this fucntion err with be catch at gloabal exception
            var users = JsonConvert.DeserializeObject<ApiSuccessResult<UserViewModel>>(body);

            return users;
        }
        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest updateRequest)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(updateRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/users/{id}", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        // delete
        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.DeleteAsync($"/api/users/{id}");

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/users/{id}/roles", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}
