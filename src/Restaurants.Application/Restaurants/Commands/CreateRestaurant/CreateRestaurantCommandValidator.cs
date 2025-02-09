using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(createRestaurant => createRestaurant.Name)
            .Length(3, 100);

        RuleFor(CreateRestaurantDto => CreateRestaurantDto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories");

        RuleFor(CreateRestaurantDto => CreateRestaurantDto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(CreateRestaurantDto => CreateRestaurantDto.PostalCode)
            .Matches("^\\d{2}-\\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX).");

    }
}
