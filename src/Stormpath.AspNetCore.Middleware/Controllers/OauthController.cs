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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.Controllers
{
    public sealed class OauthController : AbstractControllerMiddleware
    {
        private readonly static string[] SupportedMethods = { "GET" };
        private readonly static string[] SupportedContentTypes = { "application/json" };

        public OauthController(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IClient client,
            StormpathConfiguration configuration,
            string path)
            : base(next, loggerFactory, client, configuration, path, SupportedMethods, SupportedContentTypes)
        {
        }

        protected override async Task GetJson(HttpContext context, IClient scopedClient)
        {
            await context.Response.WriteAsync("tokens!");
        }
    }
}
