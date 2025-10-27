using FluentValidation;
using GroceryShop.Api.Models;
using GroceryShop.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var (statusCode, message, type) = context.Exception switch
        {
            ValidationException ex => (
                HttpStatusCode.BadRequest,
                ex.Message,
                "Validation Error"
            ),

            ForbiddenException ex => (
                HttpStatusCode.Forbidden,
                ex.Message,
                "Forbidden"
            ),

            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "Resource not found",
                "Not Found"
            ),

            InvalidOperationException => (
                HttpStatusCode.Conflict,
                "A conflict occurred processing your request",
                "Conflict"
            ),

            SecurityException ex => (
                HttpStatusCode.Forbidden,
                ex.Message,
                "Forbidden"
            ), 

            _ => (
                HttpStatusCode.InternalServerError,
                "Something went wrong. Our team is already working on it!",
                "Server Error"
            )
        };

        _logger.LogError(context.Exception, "Unhandled exception occurred");

        var response = new ErrorResponse
        {
            Type = type,
            Message = message,
            Details = null, // hiding internal errors from user
            TraceId = context.HttpContext.TraceIdentifier
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = (int)statusCode
        };

        context.ExceptionHandled = true;
    }
}
