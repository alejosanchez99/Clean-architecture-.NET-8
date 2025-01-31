using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infraestructure.Authorization.Requirements;

public class MinimumAgeRequirement(int mimiumAge) : IAuthorizationRequirement
{
    public int MimiumAge { get; } = mimiumAge;
}
