using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Appication.System.Language
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _config;
        private readonly EShopDBContext _dbContext;

        public LanguageService(EShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            var LanguageList = await _dbContext.Languages.Select(x => new LanguageViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return new ApiSuccessResult<List<LanguageViewModel>>(LanguageList);
        }
    }
}
