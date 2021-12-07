using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;

namespace eShopSolution.Appication.System.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDBContext _context;

        public CategoryService(EShopDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCategory(CUCategoryRequest request)
        {
            List<CategoryTranslation> allTranslations = new List<CategoryTranslation>();
            var allLanguages = _context.Languages;
            foreach(var ct in allLanguages)
            {
                if(ct.Id == request.languageId)
                {
                    allTranslations.Add(new CategoryTranslation() { 
                        LanguageId = request.languageId,
                        Name = request.Name,
                        SeoAlias = request.SeoAlias,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle
                    });
                }
                else
                {
                    allTranslations.Add(new CategoryTranslation()
                    {
                        LanguageId = ct.Id,
                        Name = "N/A",
                        SeoAlias = "N/A",
                        SeoDescription = "N/A",
                        SeoTitle = "N/A"
                    });
                }
            }

            Category category = new Category()
            {
                categoryTranslations = allTranslations,
                IsShowOnHome = true,
                Status = Data.Enums.Status.Active,              
            };

            if(request.parentId != 0)
            {
                category.ParentID = request.parentId;
            }

            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;      
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
                ParentID = x.c.ParentID
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
                ParentID = x.c.ParentID==null?0: x.c.ParentID,
                SeoAlias = x.ct.SeoAlias,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle
            }).FirstOrDefaultAsync();
        }
    }
}
