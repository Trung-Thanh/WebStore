using eShopSolution.ViewModels.System.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace aShopSolution.AdminApp.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            //use api from WebApi
            client.BaseAddress = new Uri("https://localhost:5001");

            // post request to uri of WebApi
            var response = await client.PostAsync("/api/users/authenticate", httpContent);

            //get tocken form api
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}
