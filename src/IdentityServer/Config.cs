// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string> {"role"}
                    ),
                new IdentityResource(
                    "country",
                    "The country where you live",
                    new List<string> {"country"}
                    ),
                new IdentityResource(
                    "subscriptionlevel",
                    "Your subscription level",
                    new List<string> { "subscriptionlevel" }
                    )
            };

        public static IEnumerable<ApiResource> ApiResourcs =>
            new ApiResource[]
            {
                new ApiResource(
                    "textgalleryapi1",
                    "Text Gallery API",
                    new List<string> {"role"})
                        {
                            ApiSecrets = { new Secret("apisecret".Sha256()) },
                            Scopes = { "textgalleryapi", "imagegalleryapi"}
                        },
                new ApiResource(
                    "imagegalleryapi1",
                    "Image Gallery API",
                    new List<string> {"role"})
                        {
                            ApiSecrets = { new Secret("apisecret".Sha256()) },
                            Scopes = { "textgalleryapi", "imagegalleryapi"}
                        }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("textgalleryapi"),
                 new ApiScope("imagegalleryapi")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName ="Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris= new List<string>()
                    {
                        "https://localhost:44389/signin-oidc"
                    },
                    PostLogoutRedirectUris=new List<string>()
                    {
                        "https://localhost:44389/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "textgalleryapi",
                        "imagegalleryapi",
                        "country",
                        "subscriptionlevel"
                    },
                    RequirePkce=false,
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256())
                    }
                },
                new Client
                {
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 20,
                    AllowOfflineAccess = true,
                    //RefreshTokenExpiration = TokenExpiration.Sliding, //Default Absolute
                    //SlidingRefreshTokenLifetime = 3600,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName ="Text Gallery",
                    ClientId = "textgalleryclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris= new List<string>()
                    {
                        "https://localhost:44363/signin-oidc"
                    },
                    PostLogoutRedirectUris=new List<string>()
                    {
                        "https://localhost:44389/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "textgalleryapi",
                        "imagegalleryapi",
                        "textgalleryapi1",
                        "imagegalleryapi1",
                        "country",
                        "subscriptionlevel"
                    },
                    RequirePkce=false,
                    ClientSecrets=
                    {
                        new Secret("secret2".Sha256())
                    }
                },
                 new Client
                {
                    ClientId = "clientClientCredentials",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                     AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "textgalleryapi",
                        "imagegalleryapi",
                        "textgalleryapi1",
                        "imagegalleryapi1",
                        "country",
                        "subscriptionlevel"
                    },
                },
                 new Client
                {
                    ClientId = "clientCodeAndClientCredentials",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    // scopes that client has access to
                     AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "textgalleryapi",
                        "imagegalleryapi",
                        "textgalleryapi1",
                        "imagegalleryapi1",
                        "country",
                        "subscriptionlevel"
                    },
                },
                 new Client
                {
                    ClientId = "clientResourceOwnerPassword",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    // scopes that client has access to
                     AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "textgalleryapi",
                        "imagegalleryapi",
                        "textgalleryapi1",
                        "imagegalleryapi1",
                        "country",
                        "subscriptionlevel"
                    },
                },
                 new Client
                {
                    ClientId = "clientResourceOwnerPasswordAndClientCredentials",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    // scopes that client has access to
                     AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "textgalleryapi",
                        "imagegalleryapi",
                        "textgalleryapi1",
                        "imagegalleryapi1",
                        "country",
                        "subscriptionlevel"
                    },
                },
            };
    }
}