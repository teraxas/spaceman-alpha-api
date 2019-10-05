using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spaceman.Service.Services;
using Spaceman.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service
{
    public static class Configurator
    {
        public static IServiceCollection AddSpacemanService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(typeof(MapperProfile));
            services.Configure<Service.SpacemanServiceOptions>(configuration.GetSection("SpacemanService"));

            services.AddSingleton<MongoProvider>();
            services.AddSingleton<IPlayerService, PlayerService>();
            services.AddSingleton<ILocationService, LocationService>();

            return services;
        }
    }
}
