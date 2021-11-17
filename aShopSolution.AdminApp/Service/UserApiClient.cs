using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;
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

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            //use api from WebApi
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // post request to uri of WebApi
            var response = await client.PostAsync("/api/users/authenticate", httpContent);

            //get tocken form api
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<PageResult<UserViewModel>> GetUsersPagings(GetUserPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // authentication the token stored on header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);

            var response = await client.GetAsync($"/api/users/paging?pageIndex=" +
                $"{request.pageIndex}&pageSize={request.pageSize}&keyword={request.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PageResult<UserViewModel>>(body);

            return users;
        }

        public async Task<bool> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // needless to authenticate the tocken

            var json = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/users", httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}
