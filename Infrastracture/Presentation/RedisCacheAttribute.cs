using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Presentation
{
    internal class RedisCacheAttribute(int durationInSeconds = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManger>().cacheService;

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var result = await cacheService.GetCachedValueAsync(cacheKey);
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.OK
                };
                return;
            }

            var resultContext = await next.Invoke();

            if (resultContext.Result is OkObjectResult objectResult)
            {
                var serialized = JsonSerializer.Serialize(objectResult.Value);
                await cacheService.SetCacheValueAsync(cacheKey, serialized, TimeSpan.FromSeconds(durationInSeconds));
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            /// api/Products?sort=priceAsc&pageIndex=1&pageSize=5
            /// api/Products|sort-priceAsc|pageIndex-1|pageSize-5

            var keyBuilder = new StringBuilder();

            // RequestPath api/Products
            // RequestQuery ==> Query String

            keyBuilder.Append(request.Path);

            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"|{item.Key}-{item.Value}");
            }

            return keyBuilder.ToString();
        }
    }
}