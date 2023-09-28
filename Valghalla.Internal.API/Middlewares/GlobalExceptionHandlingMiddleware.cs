using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Exceptions;

namespace Valghalla.Internal.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger logger;
        private readonly IWebHostEnvironment env;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                HandleException(context, exception);
            }
        }

        private async void HandleException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = GetErrorCode(exception);

            context.Response.ContentType = "application/json";

            if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError &&
                context.Response.StatusCode == (int)HttpStatusCode.Unauthorized &&
                env.IsProduction())
            {
                // Add specific message to body to prevent leakeage of sensitive information.
                ProblemDetails problem = new()
                {
                    Type = "Unknown error",
                    Title = "Server error",
                    Detail = "An Internal server error has occured"
                };
                logger.LogError(exception, exception.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
            else
            {
                var response = Response.FailWithError(exception.Message, GetErrors(exception));
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
        {
            IReadOnlyDictionary<string, string[]>? errors = null;

            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors.GroupBy(
                        x => x.PropertyName,
                        x => x.ErrorMessage,
                        (propertyName, errorMessages) => new
                        {
                            Key = propertyName,
                            Values = errorMessages.Distinct().ToArray()
                        })
                    .ToDictionary(x => x.Key, x => x.Values);
            }
            else if (exception is UserException userException)
            {
                errors = new Dictionary<string, string[]>
                {
                    { "user", new string[] { userException.InnerException?.Message ?? exception.Message } }
                };
            }

            return errors;
        }

        private static int GetErrorCode(Exception exception) =>
            exception switch
            {
                NullReferenceException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                UserException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
