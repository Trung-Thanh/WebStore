using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Appication.System.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDBContext _context;

        public CategoryService(EShopDBContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new {c, ct};

            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentID
            }).ToListAsync();
        }

        public async Task<CategoryViewModel> GetById(int id ,string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId && c.Id == id
                        select new { c, ct };

            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentID
            }).FirstOrDefaultAsync();
        }
    }
}
