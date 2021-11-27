using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Utility.Slide;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Appication.Utility.Silde
{
    public class SlideService : ISlideService
    {
        private readonly EShopDBContext _dbContext;

        public SlideService(EShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<SlideViewModel>> GetAll()
        {
            var Slides = await _dbContext.Slides.OrderBy(x=>x.SortOrder)
                .Select(x => new SlideViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Image = x.Image,
                SortOrder = x.SortOrder,
                Url = x.Url,
                // Data.Enums.Status = x.Status
            }).ToListAsync();

            return Slides;
        }
    }
}
