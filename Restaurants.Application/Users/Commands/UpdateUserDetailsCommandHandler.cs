using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommand> logger,
                                             IUserContext userContext,
                                             IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        CurrentUser user = userContext.GetCurrentUser()!;

        logger.LogInformation("Updating user: {UserId}, with {@Request}", user, request);

        User dbUser = await userStore.FindByIdAsync(user.Id, cancellationToken)
                      ?? throw new NotFoundException(nameof(User), user.Id);

        dbUser.Nationality = request.Nationality;
        dbUser.DateOfBirth = request.DateOfBirth;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
