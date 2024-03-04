using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer.Configuration
{
    public static class IdentityConfig
    {
        public const string Admin = "Admin";
        public const string Custumer = "Custumer";

        public static IEnumerable<IdentityResource> IdentityResource =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>()
            {
                new ApiScope("geek_shopping", "GeekShopping Server"),
                new ApiScope(name: "read", "Read data."),
                new ApiScope(name: "write", "Write data."),
                new ApiScope(name: "delete", "delete data.")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>()
            {
                new Client()
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>()
                    {
                        "read", "write", "profile"
                    }
                }
            };
    }
}
