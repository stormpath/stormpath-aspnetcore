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

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using Stormpath.Owin.Common;
using Stormpath.SDK.Account;

namespace Stormpath.Owin.CoreHarness
{
    public sealed class StormpathAuthenticationHandler : AuthenticationHandler<StormpathAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            object schemeValue;
            string scheme;

            Context.Items.TryGetValue(OwinKeys.StormpathUserScheme, out schemeValue);
            scheme = schemeValue as string;

            if (!string.Equals(scheme, Options.AuthenticationScheme, System.StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Failed("Invalid authentication scheme."));
            }

            object accountValue;
            IAccount authenticatedAccount;

            Context.Items.TryGetValue(OwinKeys.StormpathUser, out accountValue);
            authenticatedAccount = accountValue as IAccount;

            if (authenticatedAccount == null)
            {
                return Task.FromResult(AuthenticateResult.Failed("No account found."));
            }


            var principal = CreatePrincipal(authenticatedAccount, scheme);
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private static ClaimsPrincipal CreatePrincipal(IAccount account, string scheme)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, account.Email));
            claims.Add(new Claim(ClaimTypes.GivenName, account.GivenName));
            claims.Add(new Claim(ClaimTypes.Surname, account.Surname));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, account.Username));

            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
