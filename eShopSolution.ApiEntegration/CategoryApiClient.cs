using eShopColution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.ApiEntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
           IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> CreateCategory(CUCategoryRequest request)
        {
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();


            requestContent.Add(new StringContent(languageId), "languageId");

            if (request.parentId != 0)
            {
                requestContent.Add(new StringContent(request.parentId.ToString()), "parentId");
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoDescription) ? "" : request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoTitle) ? "" : request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoAlias) ? "" : request.SeoAlias.ToString()), "seoAlias");

            var response = await client.PostAsync($"/api/categories", requestContent);

            //var body = await response.Content.ReadAsStringAsync();
            // return product Id
            //int productId = (int)JsonConvert.DeserializeObject(body, typeof(int));
            //return productId;

            return response.IsSuccessStatusCode;
        }


        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            // may be the pager get param from api server url
            return await GetAsync<List<CategoryViewModel>>($"/api/categories?languageId={languageId}", true);
        }

        public async Task<CategoryViewModel> GetById(int id, string languageId)
        {
            return await GetAsync<CategoryViewModel>($"/api/categories/{id}/{languageId}", true);
        }

    }
}
