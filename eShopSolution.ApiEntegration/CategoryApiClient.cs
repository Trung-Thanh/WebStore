﻿using eShopSolution.ViewModels.Catalog.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiEntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
           IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            // may be the pager get param from api server url
            return await GetAsync<List<CategoryViewModel>>($"/api/categories?languageId={languageId}", true);
        }

        public async Task<CategoryViewModel> GetById(int id, string languageId)
        {
            return await GetAsync<CategoryViewModel>($"/api/categories/id={id}/languageId={languageId}", true);
        }
    }
}
