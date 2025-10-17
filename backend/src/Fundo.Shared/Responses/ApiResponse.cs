﻿namespace Fundo.Shared.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public ApiResponse(T data, string? message = null)
    {
        Success = true;
        Data = data;
        Message = message;
    }


    public ApiResponse()
    {
    }

    public ApiResponse(string message)
    {
        Success = false;
        Message = message;
    }
}