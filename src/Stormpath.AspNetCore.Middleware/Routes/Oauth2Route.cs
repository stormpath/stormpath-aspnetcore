// <copyright file="StormpathMiddlewareExtensions.cs" company="Stormpath, Inc.">
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
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.Routes
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

        protected override Task PostJson(HttpContext context, IClient scopedClient)
        {
            if (!context.Request.HasFormContentType)
            {
                return CreateOauthError(context, "invalid_request");
            }

            var grantType = context.Request.Form["grant_type"].ToString();
            var username = context.Request.Form["username"].ToString();
            var password = context.Request.Form["password"].ToString();

            if (string.IsNullOrEmpty(grantType))
            {
                return CreateOauthError(context, "invalid_request");
            }

            if (grantType.Equals("client_credentials", StringComparison.OrdinalIgnoreCase))
            {
                return ExecuteClientCredentialsFlow(context, username, password);
            }
            else if (grantType.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                return ExecutePasswordFlow(context, username, password);
            }
            else
            {
                return CreateOauthError(context, "unsupported_grant_type");
            }
        }

        private static Task ExecuteClientCredentialsFlow(HttpContext context, string username, string password)
        {
            throw new NotImplementedException();
        }

        private static Task ExecutePasswordFlow(HttpContext context, string username, string password)
        {
            throw new NotImplementedException();
        }

        private static Task CreateOauthError(HttpContext context, string message)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json;charset=UTF-8";
            context.Response.Headers["Cache-Control"] = "no-store";
            context.Response.Headers["Pragma"] = "no-cache";

            var error = new
            {
                error = message
            };

            return context.Response.WriteAsync(Serializer.Serialize(error), Encoding.UTF8);
        }
    }
}
