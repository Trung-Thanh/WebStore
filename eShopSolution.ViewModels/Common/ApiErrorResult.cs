using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationErrors { get; set; }
        public ApiErrorResult(string mess)
        {
            isSuccessed = false;
            message = mess;
        }

        public ApiErrorResult(string[] validationErrors)
        {
            isSuccessed = false;
            ValidationErrors = validationErrors;
        }
    }
}
