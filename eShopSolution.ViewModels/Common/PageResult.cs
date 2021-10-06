using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class PageResult<T>
    { 
        public List<T> Items { set; get; }

        public int totalRecord { set; get; }
    }
}
