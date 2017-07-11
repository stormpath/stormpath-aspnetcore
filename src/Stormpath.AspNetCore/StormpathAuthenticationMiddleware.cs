// <copyright file="StormpathAuthenticationMiddleware.cs" company="Stormpath, Inc.">
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
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Stormpath.Owin.Abstractions.Configuration;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathAuthenticationMiddleware : AuthenticationMiddleware<StormpathAuthenticationOptions>
    {
        private readonly IntegrationConfiguration _config;
        private readonly ILogger _logger;

        public StormpathAuthenticationMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            IOptions<StormpathAuthenticationOptions> options,
            IntegrationConfiguration integrationConfiguration,
            ILogger logger)
            : base(next, options, loggerFactory, encoder)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (integrationConfiguration == null)
            {
                throw new ArgumentNullException(nameof(integrationConfiguration));
            }

            _config = integrationConfiguration;
            _logger = logger ?? NullLogger.Instance;
        }

        protected override AuthenticationHandler<StormpathAuthenticationOptions> CreateHandler()
        {
            return new StormpathAuthenticationHandler(_config, _logger);
        }
    }
}
