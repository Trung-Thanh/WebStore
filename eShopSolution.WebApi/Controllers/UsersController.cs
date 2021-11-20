using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Appication.System.User;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous] //chưa đăng nhập vẫn dùng được hàm này
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultToken = await _userService.Authenticate(request);
            if (!resultToken.isSuccessed)
            {
                return BadRequest(resultToken);
            }

            // return called "response"
            return Ok(resultToken);
        }

        [HttpPost]
        // AllowAnonymous allow we use this method without login
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result.isSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //http://localhost/api/users/paging?pageIndex=1&pageSize=10&keyword=
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            var products = await _userService.GetUsersPaging(request);
            return Ok(products);
        }

        //update

        //http://localhost/api/users/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.GetUserById(id);
            if (result.isSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // https://localhost/api/user/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Update(id, request);
            if (!result.isSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Delete(id);
            if (result.isSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // role assign
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RoleAssign(id, request);
            if (!result.isSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
