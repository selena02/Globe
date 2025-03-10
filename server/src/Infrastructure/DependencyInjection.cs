﻿using Application.Common.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddApplicationDb(config);
            services.AddIdentity();
            services.AddJwtAuthentication(config);
            services.AddRoleBasedAuthorization();
            services.AddCloudinary();
            services.AddGeocoding();
            services.AddHttpClient<ILandmarkService, LandmarkService>();
            
            return services;
        }
    }
}
