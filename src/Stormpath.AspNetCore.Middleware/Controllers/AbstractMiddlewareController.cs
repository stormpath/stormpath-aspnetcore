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
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.Controllers
{
    public abstract class AbstractMiddlewareController
    {
        protected readonly RequestDelegate _next;
        protected readonly ILogger _logger;
        protected readonly StormpathConfiguration _config;
        private readonly IClient _client;

        public AbstractMiddlewareController(RequestDelegate next, ILoggerFactory loggerFactory, IClient client, StormpathConfiguration configuration)
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
            _logger = loggerFactory.CreateLogger<StormpathMiddleware>();
            _config = configuration;
            _client = client;
        }

        public async Task Invoke(HttpContext context)
        {
            
        }
    }
}
