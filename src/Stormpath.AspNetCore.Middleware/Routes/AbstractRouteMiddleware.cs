// <copyright file="AbstractMiddlewareController.cs" company="Stormpath, Inc.">
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
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Stormpath.AspNetCore.Internals;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.Routes
{
    public abstract class AbstractRouteMiddleware
    {
        private readonly IScopedClientFactory _clientFactory;
        private readonly string _path;
        private readonly string[] _supportedMethods;
        private readonly string[] _supportedContentTypes;

        protected readonly RequestDelegate _next;
        protected readonly ILogger _logger;
        protected readonly StormpathConfiguration _config;

        public AbstractRouteMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IClient client,
            StormpathConfiguration configuration,
            string path,
            IEnumerable<string> supportedMethods,
            IEnumerable<string> supportedContentTypes)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _next = next;
            _logger = loggerFactory.CreateLogger<AbstractRouteMiddleware>();
            _config = configuration;
            _clientFactory = new ScopedClientFactory(client);
            _path = path;
            _supportedMethods = supportedMethods.ToArray();
            _supportedContentTypes = supportedContentTypes.ToArray();
        }

        public Task Invoke(HttpContext context)
        {
            if (!IsSupportedRequest(context))
            {
                return _next.Invoke(context);
            }

            _logger.LogInformation($"Stormpath middleware handling request {context.Request.Path}");

            var scopedClient = CreateScopedClient(context);

            return Dispatch(context, scopedClient);
        }

        private bool IsSupportedRequest(HttpContext context)
        {
            bool supportedVerb = _supportedMethods.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase);

            bool matchesPath = context.Request.Path.StartsWithSegments(_path);

            //todo
            //bool hasAccept = !string.IsNullOrEmpty(context.Items["Stormpath.Accept"].ToString());

            return supportedVerb
                && matchesPath;
                //&& hasAccept;
        }

        private IClient CreateScopedClient(HttpContext context)
        {
            var fullUserAgent = CreateFullUserAgent(context);

            var scopedClientOptions = new ScopedClientOptions()
            {
                UserAgent = fullUserAgent
            };

            return _clientFactory.Create(scopedClientOptions);
        }

        private static string CreateFullUserAgent(HttpContext context)
        {
            var userAgentBuilder = (IFrameworkUserAgentBuilder)context.ApplicationServices.GetService(typeof(IFrameworkUserAgentBuilder));

            var callingAgent = string
                .Join(" ", context.Request.Headers["X-Stormpath-Agent"])
                .Trim();

            return string
                .Join(" ", callingAgent, userAgentBuilder.UserAgent)
                .Trim();
        }

        private Task Dispatch(HttpContext context, IClient scopedClient)
        {
            var method = context.Request.Method;
            var targetContentType = SelectBestContentType(context.Request.Headers["Accept"]);

            if (targetContentType == "application/json")
            {
                if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    return GetJson(context, scopedClient);
                }

                if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    return PostJson(context, scopedClient);
                }

                throw new Exception($"Unknown verb to Stormpath middleware: '{method}'.");
            }
            else if (targetContentType == "text/html")
            {
                if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    return GetHtml(context, scopedClient);
                }

                if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    return PostHtml(context, scopedClient);
                }

                throw new Exception($"Unknown verb to Stormpath middleware: '{method}'.");
            }

            throw new Exception($"Unknown target Content-Type: '{targetContentType}'.");
        }

        private string SelectBestContentType(IEnumerable<string> acceptedContentTypes)
        {
            // todo - spec-compliant content-type negotiation
            foreach (var contentType in acceptedContentTypes)
            {
                if (_supportedContentTypes.Contains(contentType))
                {
                    return contentType;
                }
            }

            return _supportedContentTypes.First();
        }

        protected virtual Task GetJson(HttpContext context, IClient scopedClient)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support GET with application/json.");
        }

        protected virtual Task GetHtml(HttpContext context, IClient scopedClient)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support GET with text/html.");
        }

        protected virtual Task PostJson(HttpContext context, IClient scopedClient)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support POST with application/json.");
        }

        protected virtual Task PostHtml(HttpContext context, IClient scopedClient)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support POST with text/html.");
        }
    }
}
