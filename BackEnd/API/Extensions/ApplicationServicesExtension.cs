using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP.UnitOfWork;
using AspNetCoreRateLimit;
using Domain.Interfaces;

namespace API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.ClientWhitelist = new List<string>();

                options.GeneralRules = new List<RateLimitRule>
                {
                        new() {
                            Endpoint = "*",
                            Period = "10s",
                            Limit = 2
                        }
                };
            });
        }
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}