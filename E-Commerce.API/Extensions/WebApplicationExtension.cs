using Domain.Contracts;
using E_Commerce.API.MiddleWares;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtension
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
            await dbIntializer.IntializeAsync();
            return app;
        }

        public static WebApplication UseCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }

    }
}
