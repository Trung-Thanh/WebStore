using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Utility.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebStore.Models
{
    public class HomeViewModel
    {
        public List<SlideViewModel> Slides { get; set; }

        public List<CMProductViewModel> FeaturedProducts { get; set; }

        public List<CMProductViewModel> LatestProducts { get; set; }
    }
}
