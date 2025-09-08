using Services.Abstraction;
using Services;
using Shared;

namespace E_Commerce.API.Extensions
{
    public static class CoreServiceExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Services.AssemblyService).Assembly);
            services.AddScoped<IServiceManger, ServiceManger>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
    }
}
