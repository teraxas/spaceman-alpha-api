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
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

            var appSettings = Configuration.GetSection("Spaceman").Get<Options>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IPlayerService>();
                        var userId = context.Principal.Identity.Name;
                        var user = await userService.GetByUsername(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Spaceman API", Version = "v1" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[]{ } }
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
            });

            services.Configure<Options>(Configuration.GetSection("Spaceman"));
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

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

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
