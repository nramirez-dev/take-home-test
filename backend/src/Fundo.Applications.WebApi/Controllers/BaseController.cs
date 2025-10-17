using System.Collections.Generic;
using Fundo.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.Applications.WebApi.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> Success<T>(T data, string? message = null)
    {
        return Ok(new ApiResponse<T>(data)
        {
            Success = true,
            Message = message
        });
    }


    protected ActionResult<ApiResponse<T>> Created<T>(T data, string? message = null)
    {
        return StatusCode(201, new ApiResponse<T>(data)
        {
            Success = true,
            Message = message ?? "Resource created successfully."
        });
    }


    protected ActionResult<ApiResponse<object>> Fail(string message, int statusCode = 400)
    {
        return StatusCode(statusCode, new ApiResponse<object>(null)
        {
            Success = false,
            Message = message
        });
    }


    protected ActionResult<ApiResponse<IEnumerable<string>>> Fail(IEnumerable<string> errors, int statusCode = 400)
    {
        return StatusCode(statusCode, new ApiResponse<IEnumerable<string>>(errors)
        {
            Success = false,
            Message = "Validation failed"
        });
    }
}