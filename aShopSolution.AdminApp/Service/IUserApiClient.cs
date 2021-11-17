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
        Task<string> Authenticate(LoginRequest request);

        Task<PageResult<UserViewModel>> GetUsersPagings(GetUserPagingRequest request);

        Task<bool> RegisterUser(RegisterRequest registerRequest);
    }
}
