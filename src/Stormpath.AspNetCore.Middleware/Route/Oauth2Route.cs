// <copyright file="Oauth2Route.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
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
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Stormpath.AspNetCore.Internal;
using Stormpath.AspNetCore.Model.Error;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;
using Stormpath.SDK.Oauth;

namespace Stormpath.AspNetCore.Route
{
    public sealed class Oauth2Route : AbstractRouteMiddleware
    {
        private readonly static string[] SupportedMethods = { "POST" };
        private readonly static string[] SupportedContentTypes = { "application/json" };

        public Oauth2Route(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IScopedClientFactory clientFactory,
            StormpathConfiguration configuration,
            string path)
            : base(next, loggerFactory, clientFactory, configuration, path, SupportedMethods, SupportedContentTypes)
        {
        }

        protected override Task PostJson(HttpContext context, IClient client)
        {
            if (!context.Request.HasFormContentType)
            {
                return Error.Create<OauthInvalidRequest>(context);
            }

            var grantType = context.Request.Form["grant_type"].ToString();
            var username = context.Request.Form["username"].ToString();
            var password = context.Request.Form["password"].ToString();

            if (string.IsNullOrEmpty(grantType))
            {
                return Error.Create<OauthInvalidRequest>(context);
            }

            if (grantType.Equals("client_credentials", StringComparison.OrdinalIgnoreCase))
            {
                return ExecuteClientCredentialsFlow(context, username, password);
            }
            else if (grantType.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                return ExecutePasswordFlow(context, client, username, password);
            }
            else
            {
                return Error.Create<OauthUnsupportedGrant>(context);
            }
        }

        private static Task ExecuteClientCredentialsFlow(HttpContext context, string username, string password)
        {
            throw new NotImplementedException();
        }

        private async Task ExecutePasswordFlow(HttpContext context, IClient client, string username, string password)
        {
            var application = await client.GetApplicationAsync(_configuration.Application.Href);

            var passwordGrantRequest = OauthRequests.NewPasswordGrantRequest()
                .SetLogin(username)
                .SetPassword(password)
                .Build();

            var tokenResult = await application.NewPasswordGrantAuthenticator()
                .AuthenticateAsync(passwordGrantRequest);

            var sanitizer = new ResponseSanitizer<IOauthGrantAuthenticationResult>();
            var responseModel = sanitizer.Sanitize(tokenResult);

            await Response.Ok(responseModel, context);
        }
    }
}
