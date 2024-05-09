using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public class SeedData
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            string roleName = role.ToString();
            
            var roleExist = await roleManager.RoleExistsAsync(roleName);

            if (roleExist)
            {
                continue;
            }
            
            var roleResult = await roleManager.CreateAsync(new Role { Name = roleName });

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"Error seeding '{roleName}' role");
            }
        }
    }
}