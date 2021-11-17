using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiResult<T>
    {
        public bool isSuccessed { get; set; }

        public string message { get; set; }

        public T resultObj { get; set; }
    }
}
