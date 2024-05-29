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
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            var roleName = role.ToString();
            
            var roleExist = roleName != null && await roleManager.RoleExistsAsync(roleName);

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
        
        var pilotUser = await userManager.FindByEmailAsync("pilot@gmail.com");
        if (pilotUser != null)
        {
            if (!await userManager.IsInRoleAsync(pilotUser, "Pilot"))
            {
                var adminRoleResult = await userManager.AddToRoleAsync(pilotUser, "Pilot");
                if (!adminRoleResult.Succeeded)
                {
                    throw new InvalidOperationException("Error adding 'Pilot' role to 'Pilot' user");
                }
            }
            if (!await userManager.IsInRoleAsync(pilotUser, "Guide"))
            {
                var memberRoleResult = await userManager.AddToRoleAsync(pilotUser, "Guide");
                if (!memberRoleResult.Succeeded)
                {
                    throw new InvalidOperationException("Error adding 'Guide' role to 'Guide' user");
                }
            }
        }
    }
}