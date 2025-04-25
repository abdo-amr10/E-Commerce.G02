using Services.Abstraction;
using Services;

namespace E_Commerce.API.Extensions
{
    public static class CoreServiceExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Services.AssemblyService).Assembly);
            services.AddScoped<IServiceManger, ServiceManger>();
            return services;
        }
    }
}
