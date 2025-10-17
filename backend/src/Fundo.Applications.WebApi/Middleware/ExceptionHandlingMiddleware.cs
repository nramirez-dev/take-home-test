using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Fundo.Shared.Exceptions;
using Fundo.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fundo.Applications.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception caught.");

            var response = context.Response;
            response.ContentType = "application/json";

            var (status, message) = ex switch
            {
                NotFoundException => (HttpStatusCode.NotFound, ex.Message),
                ValidationException ve => (HttpStatusCode.BadRequest, string.Join("; ", ve.Errors)),
                BusinessException => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            response.StatusCode = (int)status;

            var result = JsonSerializer.Serialize(new ApiResponse<string>(message)
            {
                Success = false
            });

            await response.WriteAsync(result);
        }
    }
}