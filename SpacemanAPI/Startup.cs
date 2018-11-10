using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Spaceman.Service.Utilities;
using Spaceman.Service.Services;
using AutoMapper;
using Spaceman.Service.Models;

namespace Spaceman
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            MapperConfiguration = ConfigureMapper();
        }

        public IConfiguration Configuration { get; }
        public MapperConfiguration MapperConfiguration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Spaceman API", Version = "v1" });
            });

            services.Configure<Service.Options>(Configuration.GetSection("SpacemanService"));
            services.AddSingleton<IMapper>(new Mapper(MapperConfiguration));

            services.AddSingleton<MongoProvider>();
            services.AddSingleton<IPlayerService, PlayerService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }

        private MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile<Service.MapperProfile>();
            });
        }

    }
}
