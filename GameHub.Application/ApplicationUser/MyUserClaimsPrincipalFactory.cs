using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.ApplicationUser
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<Domain.Entities.ApplicationUser>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<Domain.Entities.ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Domain.Entities.ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("nickname", user.Nickname ?? ""));
            identity.AddClaim(new Claim("balance", user.Balance.ToString()));

            return identity;
        }
    }
}
