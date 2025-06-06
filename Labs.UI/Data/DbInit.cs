using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Labs.UI.Data
{
    public class DbInit
    {
        public static async Task SeedData(WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        
            // Создание администратора

            var adminEmail = "admin@gmail.com";
            var adminPassword = "123456";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser();
                await userManager.SetEmailAsync(adminUser, adminEmail);
                await userManager.SetUserNameAsync(adminUser, adminUser.Email);
                adminUser.EmailConfirmed = true;
                await userManager.CreateAsync(adminUser, adminPassword);
                var claim = new Claim(ClaimTypes.Role, "admin");
                await userManager.AddClaimAsync(adminUser, claim);               
            }

            // Создание обычного пользователя

            var userEmail = "user@gmail.com";
            var userPassword = "qwerty";

            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                user = new ApplicationUser();
                await userManager.SetEmailAsync(user, userEmail);
                await userManager.SetUserNameAsync(user, user.Email);
                user.EmailConfirmed = true;
                await userManager.CreateAsync(user, userPassword);                
            }
        }
    }
}
