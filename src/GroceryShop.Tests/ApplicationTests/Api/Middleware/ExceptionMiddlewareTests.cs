using FluentValidation;
using GroceryShop.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace GroceryShop.Tests.ApplicationTests.Api.Middleware
{
    public class ExceptionMiddlewareTests
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlingMiddleware _middleware;
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;

        public ExceptionMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();

            _next = (context) =>
                throw new KeyNotFoundException("Order not found");

            _middleware = new ExceptionHandlingMiddleware(_next, _loggerMock.Object);
        }

        [Fact]
        public async Task UnknownException_ReturnsInternalServerError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();

            RequestDelegate next = (_) =>
                throw new Exception("Unexpected error");

            var middleware = new ExceptionHandlingMiddleware(next, loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert Status Code
            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

            // Assert JSON error response
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

            Assert.Contains("An internal server error occurred", responseText);
            Assert.Contains("\"type\":\"Server Error\"", responseText);

            // Verify logger wrote the actual exception message
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.Is<Exception>(ex => ex.Message == "Unexpected error"),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }



    }

}
