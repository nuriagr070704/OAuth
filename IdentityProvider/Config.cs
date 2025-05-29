using System.Collections.Generic;
using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace IdentityProvider
{
    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "oauthClient",
                    ClientName = "Example client application using client credentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                    AllowedScopes = new List<string> {"api1.read"}
                },
                new Client
                {
                    ClientId = "oidcClient",
                    ClientName = "Example Client Application",
                    ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string> {"https://localhost:5002/signin-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "api1.read"
                    },

                    RequirePkce = true,
                    AllowPlainTextPkce = false
                },
                new Client
                {
                    ClientId = "nextjs-client",
                    ClientName = "Next.js Frontend",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    RedirectUris = { "http://localhost:3000/auth/callback" },
                    PostLogoutRedirectUris = { "http://localhost:3000/dashboard" },

                    AllowedCorsOrigins = { "http://localhost:3000" },

                    AllowedScopes = { "openid", "profile", "api1.read", "api2.read" },
                    AllowAccessTokensViaBrowser = true,
                }
            };
        }
    }

    internal class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "api1",
                    DisplayName = "API #1",
                    Description = "Allow the application to access API #1 on your behalf",
                    Scopes = new List<string> {"api1.read", "api1.write"},
                    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())}, // change me!
                    UserClaims = new List<string> {"role"}
                },
                new ApiResource
                {
                    Name = "api2",
                    DisplayName = "API #2",
                    Description = "Allow the application to access API #2 on your behalf",
                    Scopes = new List<string> {"api2.read", "api2.write"},
                    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())}, // change me!
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("api1.read", "Read Access to API #1"),
                new ApiScope("api1.write", "Write Access to API #1"),
                new ApiScope("api2.read", "Read Access to API #2"),
                new ApiScope("api2.write", "Write Access to API #2")
            };
        }
    }

    internal class Users
    {
        public static List<TestUser> Get()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "scott",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                },
                new TestUser
                {
                    SubjectId = "f54fb3be-3d73-4cf8-ad10-abd84e28c163",
                    Username = "bob",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "bob@gmail.com"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                }
            };
        }
    }
}
