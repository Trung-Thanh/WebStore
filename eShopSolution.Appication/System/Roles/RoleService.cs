using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            var roles =  await _roleManager.Roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.description
            }).ToListAsync();

            return new ApiSuccessResult<List<RoleViewModel>>(roles);
        }
    }
}
