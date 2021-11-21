using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aShopSolution.AdminApp.Service
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
