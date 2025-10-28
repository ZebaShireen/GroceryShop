using FluentAssertions;
using FluentValidation;
using GroceryShop.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using Xunit;
using Microsoft.AspNetCore.Routing;


namespace GroceryShop.Tests.ApplicationTests.Api.Filters
{
    public class ApiExceptionFilterTests
    {
        private readonly ApiExceptionFilter _filter;
        private readonly Mock<ILogger<ApiExceptionFilter>> _loggerMock;

        public ApiExceptionFilterTests()
        {
            _loggerMock = new Mock<ILogger<ApiExceptionFilter>>();
            _filter = new ApiExceptionFilter(_loggerMock.Object);
        }

        private ExceptionContext CreateExceptionContext(Exception exception)
        {
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary()
            );

            return new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = exception
            };
        }

        [Fact]
        public void ValidationException_ReturnsBadRequest()
        {
            var context = CreateExceptionContext(new ValidationException("Invalid data"));

            _filter.OnException(context);

            var result = context.Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            result.Value.Should().BeAssignableTo<ErrorResponse>();
            var response = result.Value as ErrorResponse;
            response.Type.Should().Be("Validation Error");
            response.Message.Should().NotBeNullOrEmpty();
            response.TraceId.Should().NotBeNull();

            _loggerMock.VerifyLog(LogLevel.Error);
        }

        [Fact]
        public void Forbidden_ReturnsForbidden()
        {
            var context = CreateExceptionContext(new SecurityException());

            _filter.OnException(context);

            var result = context.Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);

            (result.Value as ErrorResponse).Type.Should().Be("Forbidden");
        }

        [Fact]
        public void NotFound_ReturnsNotFound()
        {
            var context = CreateExceptionContext(new KeyNotFoundException());

            _filter.OnException(context);

            var result = context.Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            (result.Value as ErrorResponse).Type.Should().Be("Not Found");
        }




        [Fact]
        public void Conflict_ReturnsConflict()
        {
            var context = CreateExceptionContext(new InvalidOperationException());

            _filter.OnException(context);

            var result = context.Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.Conflict);

            (result.Value as ErrorResponse).Type.Should().Be("Conflict");
        }

        [Fact]
        public void UnknownException_ReturnsInternalServerError()
        {
            var context = CreateExceptionContext(new Exception("Boom!"));

            _filter.OnException(context);

            var result = context.Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            var response = result.Value as ErrorResponse;
            response.Type.Should().Be("Server Error");
            response.Message.Should().Contain("Something went wrong");
            response.TraceId.Should().NotBeNull();

            _loggerMock.VerifyLog(LogLevel.Error);
        }
    }

    public static class LoggerVerifyExtensions
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level)
        {
            logger.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }

}