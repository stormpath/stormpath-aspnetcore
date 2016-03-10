// <copyright file="StormpathMiddlewareApplicationExtensions.cs" company="Stormpath, Inc.">
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
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stormpath.AspNetCore.Internal;
using Stormpath.AspNetCore.Route;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore
{
    public static class StormpathMiddlewareApplicationExtensions
    {
        /// <summary>
        /// Adds the Stormpath middleware to the pipeline with the given options.
        /// </summary>
        /// <remarks>You must call <see cref="StormpathMiddlwareServiceExtensions.AddStormpath(IServiceCollection, object)"/> before calling this method.</remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <exception cref="InvalidOperationException">The Stormpath services have not been added to the service collection.</exception>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseStormpath(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var clientFactory = app.ApplicationServices.GetRequiredService<IScopedClientFactory>();
            var userAgentBuilder = app.ApplicationServices.GetRequiredService<IFrameworkUserAgentBuilder>();
            var config = app.ApplicationServices.GetRequiredService<StormpathConfiguration>();

            AddRoutes(app, loggerFactory, clientFactory, userAgentBuilder, config);

            return app;
        }

        private static void AddRoutes(IApplicationBuilder app, ILoggerFactory loggerFactory, IScopedClientFactory clientFactory, IFrameworkUserAgentBuilder userAgentBuilder, StormpathConfiguration config)
        {
            app.UseOwin(pipeline =>
            {
                if (config.Web.Oauth2.Enabled == true)
                {
                    pipeline(next => new Oauth2Route(next, loggerFactory, clientFactory, userAgentBuilder, config, config.Web.Oauth2.Uri).Invoke);
                }

                if (config.Web.Register.Enabled == true)
                {
                    pipeline(next => new RegisterRoute(next, loggerFactory, clientFactory, userAgentBuilder, config, config.Web.Register.Uri).Invoke);
                }
            });
        }
    }
}
