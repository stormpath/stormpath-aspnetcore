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
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.AspNetCore.Internals;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;
using Stormpath.SDK.Http;
using Stormpath.SDK.Serialization;

namespace Stormpath.AspNetCore
{
    public static class StormpathMiddlewareExtensions
    {
        public static IServiceCollection AddStormpath(this IServiceCollection services, object configuration = null)
        {
            // Construct the base framework User-Agent
            IFrameworkUserAgentBuilder userAgentBuilder = new DefaultFrameworkUserAgentBuilder();

            // Construct base client
            var client = Clients.Builder()
                .SetHttpClient(HttpClients.Create().SystemNetHttpClient())
                .SetSerializer(Serializers.Create().JsonNetSerializer())
                .SetConfiguration(configuration)
                .Build();

            // Scope it!
            // Attempt to connect and get configuration from server
            //try
            //{
            //    var tenant = client.GetCurrentTenant();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Unable to initialize Stormpath client. See the inner exception for details.", ex);
            //}

            // Make objects available to DI
            services.AddSingleton(_ => client); // inject a scoped client factory instead
            services.AddSingleton(_ => client.Configuration); // todo syntax should be cleaner after rc2
            services.AddSingleton(_ => userAgentBuilder);

            return services;
        }

        /// <summary>
        /// Adds the Stormpath middleware to the pipeline with the given options.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStormpath(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Get Stormpath client from DI
            var client = app.ApplicationServices.GetRequiredService<IClient>();
            var config = app.ApplicationServices.GetRequiredService<StormpathConfiguration>();

            return app;
        }
    }
}
