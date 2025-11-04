using BooksKeeper.Application.DTOs.Requests.IdentityRoleRequests;
using BooksKeeper.Application.Interfaces.Identity;
using BooksKepeer.WebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksKepeer.WebAPI.Controllers.IdentityControllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IRoleService _roleService;

        public AdminController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var result = await _roleService.AssignRoleAsync(request);

            return HandleResult(result);
        }
    }
}
