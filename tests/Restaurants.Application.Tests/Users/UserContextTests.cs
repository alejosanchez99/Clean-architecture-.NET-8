using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Tests.Users;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        Mock<IHttpContextAccessor> httpContextAccessorMock = new();

        DateOnly dateOfBirth = new(1990, 1, 1);
        List<Claim> claims =
            [
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@test.com"),
            new Claim(ClaimTypes.Role, UserRoles.Admin),
            new Claim(ClaimTypes.Role, UserRoles.User),
            new Claim("Nationality", "German"),
            new Claim("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
            ];

        ClaimsPrincipal user = new(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(HttpContextAccessor => HttpContextAccessor.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });

        UserContext userContext = new(httpContextAccessorMock.Object);

        CurrentUser? currentUser = userContext.GetCurrentUser();
        
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("German");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        Mock<IHttpContextAccessor> httpContextAccesorMock = new();
        httpContextAccesorMock.Setup(httpContextAccesor => httpContextAccesor.HttpContext).Returns((HttpContext?)null);

        UserContext userContext = new(httpContextAccesorMock.Object);  

        Action action = () => userContext.GetCurrentUser();

        action.Should()
              .Throw<InvalidOperationException>()
              .WithMessage("User context is not present"); 
    }
}