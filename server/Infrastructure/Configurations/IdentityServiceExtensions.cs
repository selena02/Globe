using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
