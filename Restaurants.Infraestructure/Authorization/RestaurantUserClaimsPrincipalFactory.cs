﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities;
using System.Security.Claims;

namespace Restaurants.Infraestructure.Authorization;

public class RestaurantUserClaimsPrincipalFactory(UserManager<User> userManager,
                                                    RoleManager<IdentityRole> roleManager,
                                                    IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<User, IdentityRole>(userManager, roleManager, options)
{
    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        ClaimsIdentity claimIdentity = await GenerateClaimsAsync(user);

        if (user.Nationality != null)
        {
            claimIdentity.AddClaim(new Claim(AppClaimTypes.Nationality, user.Nationality));
        }

        if (user.DateOfBirth != null)
        {
            claimIdentity.AddClaim(new Claim(AppClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
        }

        return new ClaimsPrincipal(claimIdentity);
    }
}
