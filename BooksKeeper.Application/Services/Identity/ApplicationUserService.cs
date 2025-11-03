using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs.Requests.ApplicationUserRequests;
using BooksKeeper.Application.Interfaces.Identity;
using BooksKeeper.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Services.Identity
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result<IdentityResult>> RegisterAsync(RegisterUserRequest request)
        {
            var newUser = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join("; ", errors);
                return Result<IdentityResult>.Failure(Error.Validation("USER_REGISTRATION_FAILED", errorMessage));
            }
                

            return Result<IdentityResult>.Success(result);
        }

        public async Task<Result> LoginAsync(LoginUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? await _userManager.FindByNameAsync(request.Email);

            if(user is null)
                return Result.Failure(Error.NotFound("USER_NOT_FOUND", "User with the provided email/name does not exist"));

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if(!signInResult.Succeeded)
                return Result.Failure(Error.AccessUnAuthorized("INVALID_CREDENTIALS", "The provided credentials are invalid"));

            return Result.Success();
        }
    }
}
