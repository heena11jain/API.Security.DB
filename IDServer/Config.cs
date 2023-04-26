﻿using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IDServer
{
    // This will contain all the in-memory configurations
    public class Config
    {
        //added test user, this will returns a test user with some specific JWT claims
        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "010",
                    Username = "heenaj",
                    Password = "Password#123",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Heena Jain"),
                        new Claim(JwtClaimTypes.GivenName, "Heena"),
                        new Claim(JwtClaimTypes.FamilyName, "Jain"),
                        new Claim(JwtClaimTypes.WebSite, "http://https://identityserver4.readthedocs.io"),
                    }
                }
            };

        //IdentityResource is used to add identity specific data like - email, userid, phone, address which is unique etc.
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        //Based on scopes user is Authorized to acces APIs or not.
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("myApi.read"),
                new ApiScope("myApi.write"),
            };

        //This will define API and its scopes and secret. This secret code will be hashed and will be save internally with IdentityServer
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("myApi")
                {
                    Scopes = new List<string>{ "myApi.read","myApi.write" },
                    ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
                }
            };

        //Here we will define who will be granted access to protected resources (APIs)
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "cwm.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "myApi.read" }
                },
            };
    }
}