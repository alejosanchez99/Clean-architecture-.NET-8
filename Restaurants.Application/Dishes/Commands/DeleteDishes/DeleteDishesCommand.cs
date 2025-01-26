using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public class DeleteDishesCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
