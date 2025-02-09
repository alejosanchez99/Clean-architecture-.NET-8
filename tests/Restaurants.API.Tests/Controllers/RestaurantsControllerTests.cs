using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infraestructure.Seeders;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    private readonly Mock<IRestaurantSeeder> _restaurantsSeederMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _webApplicationFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                                            _ => _restaurantsRepositoryMock.Object));


                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantSeeder),
                                            _ => _restaurantsSeederMock.Object));
            });
        });
    }

    [Fact]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        int id = 1123;

        _restaurantsRepositoryMock.Setup(restaurant => restaurant.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        HttpClient client = _webApplicationFactory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"api/restaurants/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        int id = 99;

        Restaurant restaurant = new()
        {
            Id = id,
            Name = "Test",
            Description = "Test description"
        };

        _restaurantsRepositoryMock.Setup(restaurant => restaurant.GetByIdAsync(id)).ReturnsAsync(restaurant);

        HttpClient client = _webApplicationFactory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"api/restaurants/{id}");
        RestaurantDto? restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test description");
    }


    [Fact]
    public async void GetAll_ForValidRequest_Returns200Ok()
    {
        HttpClient client = _webApplicationFactory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("api/restaurants?pageNumber=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        HttpClient client = _webApplicationFactory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("api/restaurants");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}