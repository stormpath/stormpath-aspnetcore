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
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using Stormpath.Configuration.Abstractions;
using Stormpath.Configuration.Abstractions.Model;
using Stormpath.Owin.Common;
using Stormpath.Owin.Middleware;
using Stormpath.SDK.Account;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathAuthenticationHandler : AuthenticationHandler<StormpathAuthenticationOptions>
    {
        private AuthenticationRequiredBehavior handler;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var config = Context.Items.Get<StormpathConfiguration>(OwinKeys.StormpathConfiguration);
            var scheme = Context.Items.Get<string>(OwinKeys.StormpathUserScheme);
            var account = Context.Items.Get<IAccount>(OwinKeys.StormpathUser);

            var getAcceptHeaderFunc = new Func<string>(() => Request.Headers["Accept"]);
            var getRequestPathFunc = new Func<string>(() => Request.Path);
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

            this.handler = new AuthenticationRequiredBehavior(
                config.Web,
                getAcceptHeaderFunc,
                getRequestPathFunc,
                deleteCookieAction,
                setStatusCodeAction,
                redirectAction);

            if (this.handler.IsAuthorized(scheme, Options.AuthenticationScheme, account))
            {
                var principal = CreatePrincipal(account, scheme);
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), scheme);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            
            return Task.FromResult(AuthenticateResult.Failed("Request is not properly authenticated."));
        }

        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            if (this.handler != null)
            {
                handler.OnUnauthorized();
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
