using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class GeoCodingExtensions
{
    public static IServiceCollection AddGeocoding(this IServiceCollection services)
    {
    
        services.AddScoped<IGeocodingService, GeoCodingService>();

        return services;
    }
}