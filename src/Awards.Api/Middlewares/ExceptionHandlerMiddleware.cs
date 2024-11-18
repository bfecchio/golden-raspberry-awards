using Awards.Api.Contracts;
using Awards.Application.Core.Exceptions;
using Awards.Domain.Core.Errors;
using Awards.Domain.Core.Exceptions;
using Awards.Domain.Core.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Awards.Api.Middlewares
{
    internal class ExceptionHandlerMiddleware
    {
        #region Read-Only Fields

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        #endregion

        #region Constructors

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        #endregion

        #region Private Methods

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndErrors(exception);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)httpStatusCode;

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var response = JsonSerializer.Serialize(new ApiErrorResponse(errors), serializerOptions);

            await httpContext.Response.WriteAsync(response);
        }

        private static (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error>) GetHttpStatusCodeAndErrors(Exception exception)
        {
            return exception switch
            {
                ValidationException validationException => (HttpStatusCode.BadRequest, validationException.Errors),                
                DomainException domainException => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
                _ => (HttpStatusCode.InternalServerError, new[] { DomainErrors.General.ServerError })
            };
        }

        #endregion
    }
}
