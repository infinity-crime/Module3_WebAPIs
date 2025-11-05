using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs.Requests.IdentityRoleRequests;
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
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Result> AssignRoleAsync(AssignRoleRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
                return Result.Failure(Error.NotFound("USER_NOT_FOUND", "User with the provided email does not exist"));

            if(!await _roleManager.RoleExistsAsync(request.Role))
                return Result.Failure(Error.NotFound("ROLE_NOT_FOUND", "The specified role does not exist"));

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join("; ", errors);
                return Result.Failure(Error.Validation("ROLE_ASSIGNMENT_FAILED", errorMessage));
            }

            return Result.Success();
        }
    }
}
