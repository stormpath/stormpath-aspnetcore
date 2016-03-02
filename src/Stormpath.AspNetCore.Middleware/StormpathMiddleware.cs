// <copyright file="StormpathMiddleware.cs" company="Stormpath, Inc.">
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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Stormpath.AspNetCore
{
    public class StormpathMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly StormpathWebConfig _config;

        public StormpathMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, StormpathWebConfig config)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _next = next;
            _logger = loggerFactory.CreateLogger<StormpathMiddleware>();
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            bool enabled = _config.Login.Enabled;
            bool matchesPath = request.Path.StartsWithSegments(_config.Login.Uri);
            bool supportedVerb =
                request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase)
                || request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase);

            if (!enabled || !matchesPath || !supportedVerb)
            {
                await _next.Invoke(context);
                return;
            }

            bool renderHtml =
                (request.GetTypedHeaders().Accept?.Where(a => a.MediaType == "text/html").Any() ?? false)
                && _config.Produces.Contains("text/html");

            bool renderJson =
                (request.GetTypedHeaders().Accept?.Where(a => a.MediaType == "application/json").Any() ?? false)
                && _config.Produces.Contains("application/json");

            if (!renderHtml && !renderJson)
            {
                await _next.Invoke(context);
                return;
            }

            _logger.LogInformation("Caught login attempt");

            var viewModelBuilder = new LoginViewModelBuilder(
                _config.Login,
                new TenantConfiguration(), // todo
                request.QueryString.ToString());
            var viewModel = viewModelBuilder.Build();

            if (renderHtml)
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("<b>Login, yo!</b>");
            }
            else if (renderJson)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{ 'login': 'yo!' }");
            }
        }
    }
}
