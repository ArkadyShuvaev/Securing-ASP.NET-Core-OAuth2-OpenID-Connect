﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Shuvaev.IDP
{
    public static class Config
    {
	    public static List<IdentityServer4.Test.TestUser> GetUsers()
	    {
		    return new List<TestUser>
		    {
				new TestUser
			    {
				    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
				    Username = "Frank",
				    Password = "password",

				    Claims = new List<Claim>
				    {
					    new Claim("given_name", "Frank"),
					    new Claim("family_name", "Underwood"),
						new Claim("address", "1, Main Road"),
						new Claim("role", "FreeUser"),
					    new Claim("subscriptionlevel", "FreeUser"),
					    new Claim("country", "nl")
					}
			    },
			    new TestUser
			    {
				    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
				    Username = "Claire",
				    Password = "password",

				    Claims = new List<Claim>
				    {
					    new Claim("given_name", "Claire"),
					    new Claim("family_name", "Underwood"),
						new Claim("address", "2, Big Street"),
					    new Claim("role", "PayingUser"),
					    new Claim("subscriptionlevel", "PayingUser"),
					    new Claim("country", "be")
					}
			    }
			};
	    }

	    public static IEnumerable<ApiResource> GetApiResources()
	    {
		    return new List<ApiResource>
		    {
			    new ApiResource("imagegalleryapi", "Image Gallery Api", 
				new List<string> { "role" })
			    {
				    ApiSecrets = { new Secret("apisecret".Sha256()) }
			    }
		    };
	    }

	    public static IEnumerable<IdentityResource> GetIdentityResources()
	    {
		    return new List<IdentityResource>
		    {
			    new IdentityResources.OpenId(),
			    new IdentityResources.Profile(),
				new IdentityResources.Address(),
				new IdentityResource("roles", "Your role(s)", new List<string> { "role" }),
				new IdentityResource("country", "The country you are living in", new List<string>{ "country" }),
				new IdentityResource("subscriptionlevel", "Your subscription level", new List<string>{ "subscriptionlevel" })
			};
	    }

	    public static IEnumerable<Client> GetClients()
	    {
		    return new List<Client>
		    {
			    new Client()
			    {
				    ClientName = "Image Gallery",
					ClientId = "imagegalleryclient",
					AllowedGrantTypes = GrantTypes.Hybrid,

					// IdentityTokenLifetime = 300,
					// AuthorizationCodeLifetime = 300,
					AccessTokenLifetime = 120, // TODO for the testing purpose only (120 s)
					AccessTokenType = AccessTokenType.Reference, 
					//RefreshTokenExpiration = TokenExpiration.Sliding,
					//SlidingRefreshTokenLifetime = 

					UpdateAccessTokenClaimsOnRefresh = true,
					AllowOfflineAccess = true,

					RedirectUris = 
					{
						"https://localhost:44379/signin-oidc"
					},
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Address,
						"roles",
						"imagegalleryapi",
						"country",
						"subscriptionlevel"
					},
					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					// AlwaysIncludeUserClaimsInIdToken = true,
					PostLogoutRedirectUris =
					{
						"https://localhost:44379/signout-callback-oidc"
					}

				}
		    };
	    }

    }
}
