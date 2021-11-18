using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Appication.System.User
{
    public interface IUserService
    {
        // return object string of token key
        // or return error mess
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        // return is successed
        // or return error mess
        Task<ApiResult<bool>> Register(RegisterRequest request);

        // return is successed
        // or return error mess
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        // return object paging resule of userlist
        Task<ApiResult<PageResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request);
        Task<ApiResult<UserViewModel>> GetUserById(Guid id);
    }
}
