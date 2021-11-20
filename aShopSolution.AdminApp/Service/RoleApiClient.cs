using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace aShopSolution.AdminApp.Service
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {

            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            // add token to header of http client
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/roles");

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                List<RoleViewModel> resultObj = (List<RoleViewModel>)JsonConvert.DeserializeObject(body,typeof(List<RoleViewModel>));
                return new ApiSuccessResult<List<RoleViewModel>>(resultObj);
            }

            // don't catch error beacause this fucntion err with be catch at gloabal exception
            var users = JsonConvert.DeserializeObject<ApiErrorResult<List<RoleViewModel>>>(body);

            return users;
        }
    }
}
