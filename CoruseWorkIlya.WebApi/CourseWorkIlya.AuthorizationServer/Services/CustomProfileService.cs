using CourseWork.AuthorizationServer.Models;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CourseWork.AuthorizationServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = _userManager.GetUserAsync(context.Subject).Result;
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, user.UserName),
                };

                var roles = _userManager.GetRolesAsync(user).Result;
                foreach (var role in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, role));
                }

                context.IssuedClaims.AddRange(claims);
            }

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
            => Task.CompletedTask;
    }
}
