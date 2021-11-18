using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aShopSolution.AdminApp.Service
{
    public interface IUserApiClient
    {
        // return string oject = the token
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        // return paging user oject
        Task<ApiResult<PageResult<UserViewModel>>> GetUsersPagings(GetUserPagingRequest request);

        // return is successed
        // or return error mess
        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);

        // return is successed
        // or return error mess
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest updateRequest);

        Task<ApiResult<UserViewModel>> GetUserById(Guid id);
    }
}
