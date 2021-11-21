﻿using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;
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
    public class LanguageApiClient : BaseApiClient, ILanguageApiClient
    {
        public LanguageApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            // if something bad happen, the isSuccess is false, this web maybe dead
            return await GetAsync<List<LanguageViewModel>>("/api/languages", false, false);
        }
    }
}
