using Application.Common.Interfaces;
using CloudinaryDotNet;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class CloudinaryIntegration
{
    public static IServiceCollection AddCloudinary(this IServiceCollection services)
    {
        var account = new Account(
            Environment.GetEnvironmentVariable("CLOUDINARY_NAME") ?? "CloudinaryName",
            Environment.GetEnvironmentVariable("CLOUDINARY_KEY") ?? "",
            Environment.GetEnvironmentVariable("CLOUDINARY_SECRET") ?? ""
        );

        var cloudinary = new Cloudinary(account) { Api = { Secure = true } };

        services.AddSingleton(cloudinary);
        services.AddScoped<ICloudinaryService, CloudinaryService>();

        return services;
    }
}