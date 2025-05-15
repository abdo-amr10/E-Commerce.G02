using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Error_Models;

namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            var errors = context.ModelState.Where(error => error.Value.Errors.Any()).Select(error => new ValidationError
            {
                Field = error.Key,
                Errors = error.Value.Errors.Select(e=> e.ErrorMessage)
            });

            var response = new ValidationErrorResponse
            {
                Errors = errors,
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessages = "Validation Failed"
            };

            return new BadRequestObjectResult(response);
        }
    }
}
