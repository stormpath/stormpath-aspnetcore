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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Stormpath.Configuration.Abstractions.Immutable;
using Stormpath.Owin.Abstractions;
using Stormpath.Owin.Abstractions.Configuration;
using Stormpath.Owin.Middleware;
using Stormpath.SDK.Account;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathAuthenticationHandler : AuthenticationHandler<StormpathAuthenticationOptions>
    {
        private readonly SDK.Logging.ILogger _stormpathLogger;
        private readonly IIdentityTransformer _identityTransformer;
        private readonly RouteProtector _protector;

        public StormpathAuthenticationHandler(
            IntegrationConfiguration integrationConfiguration,
            SDK.Logging.ILogger stormpathLogger,
            IIdentityTransformer identityTransformer)
        {
            _stormpathLogger = stormpathLogger;
            _identityTransformer = identityTransformer ?? new DefaultIdentityTransformer();

            _protector = CreateRouteProtector(integrationConfiguration);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var scheme = Context.Items.Get<string>(OwinKeys.StormpathUserScheme);
            var account = Context.Items.Get<IAccount>(OwinKeys.StormpathUser);
            var client = Context.Items.Get<IClient>(OwinKeys.StormpathClient);

            foreach (var potentialScheme in Options.AllowedAuthenticationSchemes)
            {
                if (!_protector.IsAuthenticated(scheme, potentialScheme, account))
                {
                    continue;
                }

                var principal = await _identityTransformer.CreatePrincipalAsync(client, account, scheme);
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), scheme);
                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Request is not properly authenticated.");
        }

        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            _protector.OnUnauthorized(Request.Headers["Accept"], Request.Path);
            return Task.FromResult(true);
        }

        protected override Task<bool> HandleForbiddenAsync(ChallengeContext context)
        {
            _protector.OnUnauthorized(Request.Headers["Accept"], Request.Path);
            return Task.FromResult(true);
        }

        private RouteProtector CreateRouteProtector(IntegrationConfiguration config)
        {
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

            return new RouteProtector(
                config.Web,
                deleteCookieAction,
                setStatusCodeAction,
                redirectAction,
                this._stormpathLogger);
        }
    }
}
