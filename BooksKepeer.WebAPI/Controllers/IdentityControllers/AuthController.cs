using BooksKeeper.Application.DTOs.Requests.ApplicationUserRequests;
using BooksKeeper.Application.Interfaces.Identity;
using BooksKepeer.WebAPI.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BooksKepeer.WebAPI.Controllers.IdentityControllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IApplicationUserService _applicationUserService;

        public AuthController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerRequest)
        {
            var result = await _applicationUserService.RegisterAsync(registerRequest);

            return HandleResult<IdentityResult>(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest loginRequest)
        {
            var result = await _applicationUserService.LoginAsync(loginRequest);

            return HandleResult(result);
        }
    }
}
