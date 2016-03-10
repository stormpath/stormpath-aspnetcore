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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Stormpath.AspNetCore.Internal;
using Stormpath.AspNetCore.Model.Error;
using Stormpath.AspNetCore.Owin;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;
using Stormpath.SDK.Oauth;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Stormpath.AspNetCore.Route
{
    public sealed class Oauth2Route : AbstractRouteMiddleware
    {
        private readonly static string[] SupportedMethods = { "POST" };
        private readonly static string[] SupportedContentTypes = { "application/json" };

        public Oauth2Route(
            AppFunc next,
            ILoggerFactory loggerFactory,
            IScopedClientFactory clientFactory,
            IFrameworkUserAgentBuilder userAgentBuilder, 
            StormpathConfiguration configuration,
            string path)
            : base(next, loggerFactory, clientFactory, configuration, userAgentBuilder, path, SupportedMethods, SupportedContentTypes)
        {
        }

        protected override async Task PostJson(IOwinEnvironment context, IClient client, CancellationToken cancellationToken)
        {
            if (!context.Request.Headers.GetString("Content-Type").StartsWith("application/x-www-form-urlencoded"))
            {
                await Error.Create<OauthInvalidRequest>(context);
                return;
            }

            var requestBody = string.Empty;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrEmpty(requestBody))
            {
                await Error.Create<OauthInvalidRequest>(context);
                return;
            }

            var formData = FormContentParser.Parse(requestBody);

            var grantType = formData.GetString("grant_type");
            var username = WebUtility.UrlDecode(formData.GetString("username"));
            var password = WebUtility.UrlDecode(formData.GetString("password"));

            if (string.IsNullOrEmpty(grantType))
            {
                await Error.Create<OauthInvalidRequest>(context);
                return;
            }

            if (grantType.Equals("client_credentials", StringComparison.OrdinalIgnoreCase))
            {
                await ExecuteClientCredentialsFlow(context, username, password, cancellationToken);
                return;
            }
            else if (grantType.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                await ExecutePasswordFlow(context, client, username, password, cancellationToken);
                return;
            }
            else
            {
                await Error.Create<OauthUnsupportedGrant>(context);
                return;
            }
        }

        private static Task ExecuteClientCredentialsFlow(IOwinEnvironment context, string username, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task ExecutePasswordFlow(IOwinEnvironment context, IClient client, string username, string password, CancellationToken cancellationToken)
        {
            var application = await client.GetApplicationAsync(_configuration.Application.Href, cancellationToken);

            var passwordGrantRequest = OauthRequests.NewPasswordGrantRequest()
                .SetLogin(username)
                .SetPassword(password)
                .Build();

            var tokenResult = await application.NewPasswordGrantAuthenticator()
                .AuthenticateAsync(passwordGrantRequest, cancellationToken);

            var sanitizer = new ResponseSanitizer<IOauthGrantAuthenticationResult>();
            var responseModel = sanitizer.Sanitize(tokenResult);

            await Response.Ok(responseModel, context, cancellationToken);
        }
    }
}
