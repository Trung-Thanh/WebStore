﻿using eShopColution.Utilities.Constants;
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

namespace aShopSolution.AdminApp.Service
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

        protected async Task<ApiResult<TResponse>> GetAsync<TResponse>(string url, bool message, bool addToken)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

            // add token to header of http client
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

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
    }
}