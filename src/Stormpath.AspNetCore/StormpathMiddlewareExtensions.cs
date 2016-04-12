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
using System.Reflection;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Owin.Abstractions;
using Stormpath.Owin.Middleware;

namespace Stormpath.AspNetCore
{
    public static class StormpathMiddlewareExtensions
    {
        /// <summary>
        /// Adds services required for Stormpath.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">Configuration options for Stormpath.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="InitializationException">There was a problem initializing Stormpath.</exception>
        public static IServiceCollection AddStormpath(this IServiceCollection services, object configuration = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddInstance(new UserConfigurationContainer(configuration));

            services.AddScoped<ScopedClientAccessor>();
            services.AddScoped<ScopedLazyUserAccessor>();

            services.AddScoped(provider => provider.GetRequiredService<ScopedClientAccessor>().GetItem());
            services.AddScoped(provider => provider.GetRequiredService<ScopedLazyUserAccessor>().GetItem());
            services.AddScoped(provider => provider.GetRequiredService<ScopedLazyUserAccessor>().GetItem().Value);

            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Stormpath.Bearer", 
                    policy => policy.AddAuthenticationSchemes(RequestAuthenticationScheme.Bearer).RequireAuthenticatedUser());
                options.AddPolicy("Stormpath.Cookie",
                    policy => policy.AddAuthenticationSchemes(RequestAuthenticationScheme.Cookie).RequireAuthenticatedUser());
            });

            return services;
        }

        /// <summary>
        /// Adds the Stormpath middleware to the pipeline.
        /// </summary>
        /// <remarks>You must call <see cref="AddStormpath(IServiceCollection, object)"/> before calling this method.</remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="InvalidOperationException">The Stormpath services have not been added to the service collection.</exception>
        public static IApplicationBuilder UseStormpath(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var suppliedConfiguration = app.ApplicationServices.GetRequiredService<UserConfigurationContainer>();
            var hostingAssembly = app.GetType().GetTypeInfo().Assembly;

            var stormpathMiddleware = StormpathMiddleware.Create(new StormpathMiddlewareOptions()
            {
                LibraryUserAgent = GetLibraryUserAgent(hostingAssembly),
                Configuration = suppliedConfiguration.Configuration,
                ViewRenderer = new PrecompiledDeferringViewRenderer(new RazorViewRenderer()),
            });

            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    stormpathMiddleware.Initialize(next);
                    return stormpathMiddleware.Invoke;
                });
            });

            app.UseMiddleware<StormpathAuthenticationMiddleware>(new StormpathAuthenticationOptions() { AuthenticationScheme = "Cookie" });
            app.UseMiddleware<StormpathAuthenticationMiddleware>(new StormpathAuthenticationOptions() { AuthenticationScheme = "Bearer" });

            return app;
        }

        private static string GetLibraryUserAgent(Assembly hostingAssembly)
        {
            var hostingVersion = hostingAssembly.GetName().Version;
            return $"aspnetcore/{hostingVersion.Major}.{hostingVersion.Minor}.{hostingVersion.Build}";
        }
    }
}
