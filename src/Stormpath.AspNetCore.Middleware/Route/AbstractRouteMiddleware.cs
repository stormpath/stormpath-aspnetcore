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
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Stormpath.AspNetCore.Route
{
    public abstract class AbstractRouteMiddleware
    {
        private readonly IScopedClientFactory _clientFactory;
        private readonly string _path;
        private readonly string[] _supportedMethods;
        private readonly string[] _supportedContentTypes;

        protected readonly AppFunc _next;
        protected readonly ILogger _logger;
        protected readonly StormpathConfiguration _configuration;
        private readonly IFrameworkUserAgentBuilder _userAgentBuilder;

        public AbstractRouteMiddleware(
            AppFunc next,
            ILoggerFactory loggerFactory,
            IScopedClientFactory clientFactory,
            StormpathConfiguration configuration,
            IFrameworkUserAgentBuilder userAgentBuilder,
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

            if (clientFactory == null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (userAgentBuilder == null)
            {
                throw new ArgumentNullException(nameof(userAgentBuilder));
            }

            _next = next;
            _logger = loggerFactory.CreateLogger<AbstractRouteMiddleware>();
            _configuration = configuration;
            _userAgentBuilder = userAgentBuilder;
            _clientFactory = clientFactory;
            _path = path;
            _supportedMethods = supportedMethods.ToArray();
            _supportedContentTypes = supportedContentTypes.ToArray();
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            if (!environment.ContainsKey(OwinKeys.RequestPath))
            {
                throw new Exception($"Invalid OWIN request. Expected {OwinKeys.RequestPath}, but it was not found.");
            }

            IOwinEnvironment owinContext = new DefaultOwinEnvironment(environment);

            if (!IsSupportedPath(owinContext))
            {
                return _next.Invoke(environment);
            }

            if (!IsSupportedVerb(owinContext))
            {
                return Error.Create<MethodNotAllowed>(owinContext);
            }

            if (!HasSupportedAccept(owinContext))
            {
                return Error.Create<NotAcceptable>(owinContext);
            }

            _logger.LogInformation($"Stormpath middleware handling request {owinContext.Request.Path}");

            using (var scopedClient = CreateScopedClient(owinContext))
            {
                return Dispatch(owinContext, scopedClient, owinContext.CancellationToken);
            }
        }

        private bool IsSupportedVerb(IOwinEnvironment context)
            => _supportedMethods.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase);

        private bool HasSupportedAccept(IOwinEnvironment context)
            => true; //todo

        private bool IsSupportedPath(IOwinEnvironment context)
            => context.Request.Path.StartsWith(_path, StringComparison.OrdinalIgnoreCase);

        private IClient CreateScopedClient(IOwinEnvironment context)
        {
            var fullUserAgent = CreateFullUserAgent(context);

            var scopedClientOptions = new ScopedClientOptions()
            {
                UserAgent = fullUserAgent
            };

            return _clientFactory.Create(scopedClientOptions);
        }

        private string CreateFullUserAgent(IOwinEnvironment context)
        {
            var callingAgent = string
                .Join(" ", context.Request.Headers.Get("X-Stormpath-Agent") ?? new string[0])
                .Trim();

            return string
                .Join(" ", callingAgent, _userAgentBuilder.GetUserAgent())
                .Trim();
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

        private Task Dispatch(IOwinEnvironment context, IClient scopedClient, CancellationToken cancellationToken)
        {
            var method = context.Request.Method;
            var targetContentType = SelectBestContentType(context.Request.Headers.Get("Accept"));

            if (targetContentType == "application/json")
            {
                if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    return GetJson(context, scopedClient, cancellationToken);
                }

                if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    return PostJson(context, scopedClient, cancellationToken);
                }

                throw new Exception($"Unknown verb to Stormpath middleware: '{method}'.");
            }
            else if (targetContentType == "text/html")
            {
                if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    return GetHtml(context, scopedClient, cancellationToken);
                }

                if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    return PostHtml(context, scopedClient, cancellationToken);
                }

                throw new Exception($"Unknown verb to Stormpath middleware: '{method}'.");
            }

            throw new Exception($"Unknown target Content-Type: '{targetContentType}'.");
        }

        protected virtual Task GetJson(IOwinEnvironment context, IClient client, CancellationToken cancellationToken)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support GET with application/json.");
        }

        protected virtual Task GetHtml(IOwinEnvironment context, IClient client, CancellationToken cancellationToken)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support GET with text/html.");
        }

        protected virtual Task PostJson(IOwinEnvironment context, IClient client, CancellationToken cancellationToken)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support POST with application/json.");
        }

        protected virtual Task PostHtml(IOwinEnvironment context, IClient client, CancellationToken cancellationToken)
        {
            // This should not happen with proper configuration.
            throw new NotImplementedException("Fatal error: this controller does not support POST with text/html.");
        }
    }
}
