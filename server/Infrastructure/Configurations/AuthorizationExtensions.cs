using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddRoleBasedAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequirePilotRole", policy => policy.RequireRole("Pilot"));
            options.AddPolicy("RequireGuideRole", policy => policy.RequireRole("Guide"));
            options.AddPolicy("RequireTravelerRole", policy => policy.RequireRole("Traveler"));
        });

        return services;
    }
}