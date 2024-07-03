using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Humanizer;
using IdentityModel;

namespace CourseWork.AuthorizationServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(name: "apiAccess", displayName: "ApiAccess")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                     ClientId = "webApp",
                     ClientName = "WebApp",
                     AllowedGrantTypes = GrantTypes.Code,
                     ClientSecrets = new List<Secret>
                     {
                         new Secret("secret!cod23123eforapplicat2ion".Sha256())
                     },
                     AllowedScopes = new List<string>
                     {
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         "apiAccess",
                     },
                     RedirectUris = new List<string>
                     {
                         "https://localhost:7002/signin-oidc"
                     },
                     PostLogoutRedirectUris = new List<string>
                     {
                         "https://localhost:7002/signout-callback-oidc"
                     },
                     AllowOfflineAccess = true,
                     RequirePkce = true,
                }
            };
    }
}
