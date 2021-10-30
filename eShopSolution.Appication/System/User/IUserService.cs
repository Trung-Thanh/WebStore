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
        Task<string> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
        Task<PageResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);
    }
}
