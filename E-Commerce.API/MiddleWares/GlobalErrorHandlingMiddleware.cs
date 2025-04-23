using System;
using System.Net;
using Domain.Exceptions;
using Shared.Error_Models;

namespace E_Commerce.API.MiddleWares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next , ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandelNotFoundEndPointAsync(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something Went Error! {exception}");
                await HandelExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandelNotFoundEndPointAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";

            var responce = new ErrorDetails
            {
                ErrorMessage = $"The End Point {httpContext.Request.Path} Not Found",
                StatusCode = (int)HttpStatusCode.NotFound
            }.ToString();

            await httpContext.Response.WriteAsync(responce);
        }

        private async Task HandelExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var Responce = new ErrorDetails
            {
                ErrorMessage = exception.Message,
                StatusCode = httpContext.Response.StatusCode
            }.ToString();

            await httpContext.Response.WriteAsync(Responce);
        }
    }
}
