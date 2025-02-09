using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Tests.Middlewares;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async void InvokeAsync_WhenNotExceptionThrown_ShouldCallNextDelegate()
    {
        Mock<ILogger<ErrorHandlingMiddleware>> loggerMock = new();
        ErrorHandlingMiddleware errorHandlingMiddleware = new(loggerMock.Object);
        DefaultHttpContext context = new();
        Mock<RequestDelegate> nextDelegateMock = new();

        await errorHandlingMiddleware.InvokeAsync(context, nextDelegateMock.Object);

        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        Mock<ILogger<ErrorHandlingMiddleware>> loggerMock = new();
        ErrorHandlingMiddleware errorHandlingMiddleware = new(loggerMock.Object);
        DefaultHttpContext context = new();
        NotFoundException notFoundException = new(nameof(Restaurant), "1");

        await errorHandlingMiddleware.InvokeAsync(context, _ => throw notFoundException);

        context.Response.StatusCode.Should().Be(404);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode403()
    {
        Mock<ILogger<ErrorHandlingMiddleware>> loggerMock = new();
        ErrorHandlingMiddleware errorHandlingMiddleware = new(loggerMock.Object);
        DefaultHttpContext context = new();
        ForbidException forbidException = new();

        await errorHandlingMiddleware.InvokeAsync(context, _ => throw forbidException);

        context.Response.StatusCode.Should().Be(403);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode500()
    {
        Mock<ILogger<ErrorHandlingMiddleware>> loggerMock = new();
        ErrorHandlingMiddleware errorHandlingMiddleware = new(loggerMock.Object);
        DefaultHttpContext context = new();
        Exception forbidException = new();

        await errorHandlingMiddleware.InvokeAsync(context, _ => throw forbidException);

        context.Response.StatusCode.Should().Be(500);
    }
}