using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<UpdateRestaurantCommand, Restaurant>();

        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(destination => destination.Address, options =>
            options.MapFrom(source => new Address
            {
                City = source.City,
                PostalCode = source.PostalCode,
                Street = source.Street
            }));

        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(destination => destination.City, option =>
                        option.MapFrom(source => source.Address == null ? null : source.Address.City))
        .ForMember(destination => destination.City, option =>
                        option.MapFrom(source => source.Address == null ? null : source.Address.PostalCode))
        .ForMember(destination => destination.City, option =>
                        option.MapFrom(source => source.Address == null ? null : source.Address.Street))
        .ForMember(destination => destination.Dishes, option =>
                        option.MapFrom(source => source.Dishes));

    }
}
