using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infraestructure.Authorization.Requirements;

public class MinimumAgeRequirement(int mimiumAge) : IAuthorizationRequirement
{
    public int MimiumAge { get; } = mimiumAge;
}
