// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace EShoping.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catelogapi"),
                new ApiScope("catelogapi.read"),
                new ApiScope("catelogapi.write"),
                new ApiScope("basketapi"),
                new ApiScope("eshopinggateway"),
            };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("Catalog", "Catelog.Api")
            {
                Scopes={ "catelogapi.read", "catelogapi.write" }
            },
            new ApiResource("Basket", "Basket.Api")
            {
                Scopes={ "basketapi" }
            },
            new ApiResource("EShopingGateway", "EShoping.Gateway")
            {
                Scopes={ "eshopinggateway" }
            }
        };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientName = "Catalog API Client",
                    ClientId = "CatalogApiClient",
                    ClientSecrets = {new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "catelogapi.read", "catelogapi.write" }
                },
                new Client
                {
                    ClientName = "Basket API Client",
                    ClientId = "BasketApiClient",
                    ClientSecrets = {new Secret("5c6eb3b4-61a7-4658-ac57-2b4591ec26d2".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "basketapi" }
                },
                new Client
                {
                    ClientName = "EShoping Gateway Client",
                    ClientId = "EShopingGatewayClient",
                    ClientSecrets = {new Secret("5c6eb3b4-61a7-4658-ac57-2b4591ec26d2".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "eshopinggateway" }
                }
            };
    }
}