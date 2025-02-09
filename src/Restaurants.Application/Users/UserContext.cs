using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Restaurants.Application.Users;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        ClaimsPrincipal? user = (httpContextAccessor?.HttpContext?.User)
            ?? throw new InvalidOperationException("User context is not present");

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        string userId = user.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)!.Value;
        string email = user.FindFirst(claim => claim.Type == ClaimTypes.Email)!.Value;
        IEnumerable<string> roles = user.Claims.Where(claim => claim.Type == ClaimTypes.Role)
                                               .Select(claim => claim.Value);
        string? nationality = user.FindFirst(claim => claim.Type == "Nationality")?.Value;
        string dateOfBirthString = user.FindFirst(claim => claim.Type == "DateOfBirth")?.Value;
        DateOnly? dateOfBirth = !string.IsNullOrEmpty(dateOfBirthString) ? DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd") : null;

        return new CurrentUser(userId, email, roles, nationality, dateOfBirth);
    }
}
