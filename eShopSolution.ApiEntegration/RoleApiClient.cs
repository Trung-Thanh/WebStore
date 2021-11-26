using eShopSolution.ApiEntegration;
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

namespace eShopSolution.ApiEntegration
{
    public class RoleApiClient : BaseApiClient, IRoleApiClient
    {

        public RoleApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            // if something bad happen, the isSuccess is false, this web maybe dead
            return await GetAsync_UseApiResult<List<RoleViewModel>>("/api/roles", true);
        }
    }
}
