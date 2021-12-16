using eShopColution.Utilities.Constants;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.ApiEntegration
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected BaseApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        protected async Task<ApiResult<TResponse>> GetAsync_UseApiResult<TResponse>(string url, bool addToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

            if (addToken)
            {
                // add token to header of http client
                var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            }

            var response = await client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                ApiSuccessResult<TResponse> resultObj = (ApiSuccessResult<TResponse>)JsonConvert.DeserializeObject(body, typeof(ApiSuccessResult<TResponse>));
                return resultObj;

                //retủJsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }

            return (ApiErrorResult<TResponse>)JsonConvert.DeserializeObject(body, typeof(ApiErrorResult<TResponse>));
        }

        protected async Task<TResponse> GetAsync<TResponse>(string url, bool addToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

            if (addToken)
            {
                // add token to header of http client
                var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            }

            var response = await client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            TResponse resultObj = (TResponse)JsonConvert.DeserializeObject(body, typeof(TResponse));

            return resultObj;
        }

        protected async Task<bool> DeleteAsync(string url, bool addToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

            if (addToken)
            {
                // add token to header of http client
                var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            }

            var response = await client.DeleteAsync(url);

            //var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }
    }
}
