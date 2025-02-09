using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private readonly int[] allowPageSizes = [5, 10, 15, 30];
    private readonly string[] allowedSortByColumnName = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Category), nameof(RestaurantDto.Description)];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(restaurant => restaurant.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(restaurant => restaurant.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", allowPageSizes)}]");

        RuleFor(restaurant => restaurant.SortBy)
            .Must(value => allowedSortByColumnName.Contains(value))
            .When(query => query.SortBy != null)
            .WithMessage($"Sort by is optional, or mus be in [{string.Join(",", allowedSortByColumnName)}]");
    }
}
