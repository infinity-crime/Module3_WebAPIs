using BooksKeeper.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BooksKepeer.WebAPI.SeedMethods
{
    // Класс для начального заполнения ролей Identity
    public static class IdentitySeed
    {
        // Метод для создания ролей "Admin" и "User", если они не существуют
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
