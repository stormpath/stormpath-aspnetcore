// <copyright file="StormpathAuthenticationHandler.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
// Portions copyright 2013 Microsoft Open Technologies, Inc. Licensed under Apache 2.0.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Stormpath.Configuration.Abstractions.Immutable;
using Stormpath.Owin.Abstractions;
using Stormpath.Owin.Middleware;
using Stormpath.SDK.Account;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathAuthenticationHandler : AuthenticationHandler<StormpathAuthenticationOptions>
    {
        private readonly SDK.Logging.ILogger stormpathLogger;
        private RouteProtector handler;

        public StormpathAuthenticationHandler(SDK.Logging.ILogger stormpathLogger)
        {
            this.stormpathLogger = stormpathLogger;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var config = Context.Items.Get<StormpathConfiguration>(OwinKeys.StormpathConfiguration);
            var scheme = Context.Items.Get<string>(OwinKeys.StormpathUserScheme);
            var account = Context.Items.Get<IAccount>(OwinKeys.StormpathUser);

            var deleteCookieAction = new Action<WebCookieConfiguration>(cookie =>
            {
                Response.Cookies.Delete(cookie.Name, new CookieOptions()
                {
                    Domain = cookie.Domain,
                    Path = cookie.Path
                });
            });
            var setStatusCodeAction = new Action<int>(code => Response.StatusCode = code);
            var redirectAction = new Action<string>(location => Response.Redirect(location));

            this.handler = new RouteProtector(
                config.Web,
                deleteCookieAction,
                setStatusCodeAction,
                redirectAction,
                this.stormpathLogger);

            if (this.handler.IsAuthenticated(scheme, Options.AuthenticationScheme, account))
            {
                var principal = CreatePrincipal(account, scheme);
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), scheme);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            
            return Task.FromResult(AuthenticateResult.Fail("Request is not properly authenticated."));
        }

        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            if (this.handler != null)
            {
                handler.OnUnauthorized(Request.Headers["Accept"], Request.Path);
            }
            
            return Task.FromResult(true);
        }

        private static ClaimsPrincipal CreatePrincipal(IAccount account, string scheme)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, account.Href));
            claims.Add(new Claim(ClaimTypes.Email, account.Email));
            claims.Add(new Claim(ClaimTypes.Name, account.Username));
            claims.Add(new Claim(ClaimTypes.GivenName, account.GivenName));
            claims.Add(new Claim(ClaimTypes.Surname, account.Surname));
            claims.Add(new Claim("FullName", account.FullName));

            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
