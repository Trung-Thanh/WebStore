using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Roles
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
