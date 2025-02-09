using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests.Users;

public class CurrentUserTests
{
    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMachingRole_ShouldReturnTrue(string roleName)
    {
        CurrentUser currentUser = new("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        bool isInRole = currentUser.IsInRole(roleName);

        isInRole.Should().BeTrue();
    }

    [Fact]
    public void IsInRole_WithMachingRole_ShouldReturnFalse()
    {
        CurrentUser currentUser = new("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        bool isInRole = currentUser.IsInRole(UserRoles.Owner);

        isInRole.Should().BeFalse();
    }

    [Fact]
    public void IsInRole_WithNoMachingRole_ShouldReturnFalse()
    {
        CurrentUser currentUser = new("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        bool isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        isInRole.Should().BeFalse();
    }
}