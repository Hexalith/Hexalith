namespace Hexalith.Infrastructure.DaprRuntime.IdentityStores.Stores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hexalith.Application.Sessions.Models;

using Microsoft.AspNetCore.Identity;

internal class UserStore : UserStoreBase<
    ApplicationUser,
    ApplicationRole,
    string,
    IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>
{
}