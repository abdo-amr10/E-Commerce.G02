using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Extensions
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });
            services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            return services;
        }
    }
}
