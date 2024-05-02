using System.Text;
using Application.Common.Interfaces;
using CloudinaryDotNet;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddCors(); 
            
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
            
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequirePilotRole", policy => policy.RequireRole("Pilot"));
                options.AddPolicy("RequireGuideRole", policy => policy.RequireRole("Guide"));
                options.AddPolicy("RequireTravelerRole", policy => policy.RequireRole("Traveler"));
            });
            
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
}
