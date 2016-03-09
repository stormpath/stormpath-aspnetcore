// <copyright file="RegisterRoute.cs" company="Stormpath, Inc.">
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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stormpath.AspNetCore.Internal;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Account;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.Route
{
    public sealed class RegisterRoute : AbstractRouteMiddleware
    {
        private readonly static string[] SupportedMethods = { "POST" };
        private readonly static string[] SupportedContentTypes = { "application/json" }; // todo

        public RegisterRoute(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IScopedClientFactory clientFactory,
            StormpathConfiguration configuration,
            string path)
            : base(next, loggerFactory, clientFactory, configuration, path, SupportedMethods, SupportedContentTypes)
        {
        }

        protected override async Task PostJson(HttpContext context, IClient scopedClient)
        {
            IDictionary<string, object> postData = null;

            using (var streamReader = new StreamReader(context.Request.Body))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                postData = serializer.Deserialize<IDictionary<string, object>>(jsonReader);
            }

            var email = postData.GetOrNull("email")?.ToString();
            var password = postData.GetOrNull("password")?.ToString();

            bool missingEmailOrPassword = string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password);
            if (missingEmailOrPassword)
            {
                throw new Exception("Missing email or password!");
            }

            var givenName = postData.GetOrNull("givenName")?.ToString() ?? "UNKNOWN";
            var surname = postData.GetOrNull("surname")?.ToString() ?? "UNKNOWN";
            var username = postData.GetOrNull("username")?.ToString();

            var application = await scopedClient.GetApplicationAsync(_configuration.Application.Href);

            var newAccount = scopedClient.Instantiate<IAccount>()
                .SetEmail(email)
                .SetPassword(password)
                .SetGivenName(givenName)
                .SetSurname(surname);

            if (!string.IsNullOrEmpty(username))
            {
                newAccount.SetUsername(username);
            }

            await application.CreateAccountAsync(newAccount);

            var sanitizer = new ResponseSanitizer<IAccount>();
            var responseModel = new
            {
                account = sanitizer.Sanitize(newAccount)
            };

            await Response.Ok(responseModel, context);
        }
    }
}
