using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

public class RestaurantsProfileTests
{
    private IMapper _mapper;

    public RestaurantsProfileTests()
    {
        MapperConfiguration mapperConfiguration = new(configuration =>
        {
            configuration.AddProfile<RestaurantsProfile>();
        });

        _mapper = mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        Restaurant restaurant = new()
        {
            Id = 1,
            Name = "Test restaurants",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            Address = new()
            {
                City = "Test city",
                Street = "Test streets",
                PostalCode = "12345"
            }
        };

        RestaurantDto restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        CreateRestaurantCommand command = new()
        {
            Name = "Test restaurants",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            City = "Test city",
            Street = "Test streets",
            PostalCode = "12345"
        };

        Restaurant restaurant = _mapper.Map<Restaurant>(command);

        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);
    }

    [Fact]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        UpdateRestaurantCommand command = new()
        {
            Id = 1,
            Name = "Test restaurants",
            Description = "Test description",
            HasDelivery = true,
        };

        Restaurant restaurant = _mapper.Map<Restaurant>(command);

        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
    }
}