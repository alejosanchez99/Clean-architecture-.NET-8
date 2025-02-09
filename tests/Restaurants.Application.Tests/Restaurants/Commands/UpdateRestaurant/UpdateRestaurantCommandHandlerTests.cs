using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System.Security.AccessControl;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new();
        _mapperMock = new();
        _restaurantRepositoryMock = new();
        _restaurantAuthorizationServiceMock = new();

        _handler = new UpdateRestaurantCommandHandler(_restaurantRepositoryMock.Object,
                                                      _loggerMock.Object,
                                                      _mapperMock.Object,
                                                      _restaurantAuthorizationServiceMock.Object);
    }
    [Fact]
    public async void Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        int restaurantId = 1;
        UpdateRestaurantCommand command = new()
        {
            Id = restaurantId,
            Name = "New Test",
            Description = "New Description",
            HasDelivery = true
        };

        Restaurant restaurant = new()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test"
        };

        _restaurantRepositoryMock.Setup(restaurantRepository => restaurantRepository.GetByIdAsync(restaurantId))
                                 .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(authorizationService => authorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
                                           .Returns(true);

        await _handler.Handle(command, CancellationToken.None);

        _restaurantRepositoryMock.Verify(restaurantRepository => restaurantRepository.SaveChanges(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map(command, restaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        int restaurantId = 2;
        UpdateRestaurantCommand command = new()
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock.Setup(restaurant => restaurant.GetByIdAsync(restaurantId))
                                 .ReturnsAsync((Restaurant?)null);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should()
        .ThrowAsync<NotFoundException>()
                 .WithMessage($"Restaurant with id: {restaurantId} doesn't exist");
    }

    [Fact]
    public async Task Handle_WithUnathorizedUser_ShouldThrowForbidException()
    {
        int restaurantId = 3;
        UpdateRestaurantCommand command = new() { Id = restaurantId };
        Restaurant restaurant = new() { Id = restaurantId };

        _restaurantRepositoryMock.Setup(restaurantRepository => restaurantRepository.GetByIdAsync(restaurantId))
                                 .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(authorizationService => authorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
                                           .Returns(false);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbidException>();
    }
}