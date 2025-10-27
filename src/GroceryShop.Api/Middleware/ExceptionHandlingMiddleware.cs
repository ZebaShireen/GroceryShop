using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using FluentValidation;
using GroceryShop.Api.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;


namespace GroceryShop.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            
            var errorResponse = new ErrorResponse
            {
                TraceId = context.TraceIdentifier
            };

            switch (exception)
            {
                case UnauthorizedAccessException unauthorizedEx:  //401
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Type = "Unauthorized";
                    errorResponse.Message = unauthorizedEx.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
                    errorResponse.Type = "Server Error";
                    errorResponse.Message = "An internal server error occurred.";
                    break;
            }

            await response.WriteAsJsonAsync(errorResponse);
        }
    }
}