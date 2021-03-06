﻿
using System.Threading.Tasks;
using Guide.Models;
using Microsoft.AspNetCore.Identity;

public enum Roles
{
    admin,
    user
}

namespace Guide.Services
{
    public class RoleInitializer
    {
        public static async Task Initialize(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            string adminEmail = "admin@admin.com";
            string adminPassword = "Qwerty123@";
            var roles = new[] {"admin", "user"};
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) is null)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await userManager.FindByEmailAsync(adminEmail) is null)
            {
                Position position = new Position() {Name = "администратор"};
                Position position1 = new Position() {Name = "сотрудник"};
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    Position = position
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}