using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj1)
        {
            isSuccessed = true;
            resultObj = resultObj1;
        }

        public ApiSuccessResult()
        {
            isSuccessed = true;
        }
    }
}
