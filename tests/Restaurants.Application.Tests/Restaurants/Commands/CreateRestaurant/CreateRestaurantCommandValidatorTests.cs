using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTests
{
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        CreateRestaurantCommand command = new()
        {
            Name = "Test",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "12-345"
        };

        CreateRestaurantCommandValidator validator = new();

        TestValidationResult<CreateRestaurantCommand> result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForValidCommand_ShouldHaveValidationErrors()
    {
        CreateRestaurantCommand command = new()
        {
            Name = "Te",
            Category = "Ita",
            ContactEmail = "@test.com",
            PostalCode = "12345"
        };

        CreateRestaurantCommandValidator validator = new();

        TestValidationResult<CreateRestaurantCommand> result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(restaurantCommand => restaurantCommand.Name);
        result.ShouldHaveValidationErrorFor(restaurantCommand => restaurantCommand.Category);
        result.ShouldHaveValidationErrorFor(restaurantCommand => restaurantCommand.ContactEmail);
        result.ShouldHaveValidationErrorFor(restaurantCommand => restaurantCommand.PostalCode);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
    {
        CreateRestaurantCommandValidator validator = new();
        CreateRestaurantCommand command = new() { Category = category };

        TestValidationResult<CreateRestaurantCommand> result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(restaurantCommand => restaurantCommand.Category);
    }

    [Theory]
    [InlineData("10220")]
    [InlineData("102-20")]
    [InlineData("10 220")]
    [InlineData("10.2 20")]
    public void Validator_ForInvalidPostCode_ShouldHaveValidationErrorsForCategoryProperty(string category)
    {
        CreateRestaurantCommandValidator validator = new();
        CreateRestaurantCommand command = new() { PostalCode = category };

        TestValidationResult<CreateRestaurantCommand> result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(restaurantCommand => restaurantCommand.PostalCode);
    }
}