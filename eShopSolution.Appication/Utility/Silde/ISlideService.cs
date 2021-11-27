using eShopSolution.ViewModels.Utility.Slide;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Appication.Utility.Silde
{
    public interface ISlideService
    {
        Task<List<SlideViewModel>> GetAll();
    }
}
