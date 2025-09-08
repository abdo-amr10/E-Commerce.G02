using System;
using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
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

            var Responce = new ErrorDetails
            {
                ErrorMessage = exception.Message,
            };

            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnAuthorizedException => (int)HttpStatusCode.Unauthorized,
                ValidationException validationException => HandelValidationException(validationException,Responce),
                _ => (int)HttpStatusCode.InternalServerError
            };

            Responce.StatusCode = httpContext.Response.StatusCode;


            await httpContext.Response.WriteAsync(Responce.ToString());
        }

        private int HandelValidationException(ValidationException validationException, ErrorDetails responce)
        {
            responce.Errors = validationException.Errors;
            return (int)HttpStatusCode.BadRequest;
        }
    }
}
