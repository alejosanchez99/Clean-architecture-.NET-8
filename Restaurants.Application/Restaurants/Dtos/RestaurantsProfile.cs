using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos
{
    public class RestaurantsProfile : Profile
    {
        public RestaurantsProfile()
        {
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
}
