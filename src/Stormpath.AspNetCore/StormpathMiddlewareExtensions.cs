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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stormpath.Owin.Abstractions;
using Stormpath.Owin.Middleware;
using Stormpath.Owin.Views.Precompiled;
using Stormpath.SDK.Application;

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
        /// <exception cref="Stormpath.Owin.InitializationException">There was a problem initializing Stormpath.</exception>
        public static IServiceCollection AddStormpath(this IServiceCollection services, object configuration = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddLogging();
            services.AddSingleton<SDK.Logging.ILogger>(
                provider => new LoggerAdapter(provider.GetRequiredService<ILoggerFactory>()));

            services.AddSingleton(new UserConfigurationContainer(configuration));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ScopedClientAccessor>();
            services.AddScoped<ScopedApplicationAccessor>();
            services.AddScoped<ScopedLazyUserAccessor>();
            services.AddScoped(provider => provider.GetRequiredService<ScopedClientAccessor>().GetItem());
            services.AddScoped(provider => provider.GetRequiredService<ScopedApplicationAccessor>().GetItem());
            services.AddScoped(provider => provider.GetRequiredService<ScopedLazyUserAccessor>().GetItem());
            services.AddScoped(provider => provider.GetRequiredService<ScopedLazyUserAccessor>().GetItem().Value);

            services.AddSingleton<RazorViewRenderer>();

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
            var logger = app.ApplicationServices.GetRequiredService<SDK.Logging.ILogger>();

            var hostingAssembly = app.GetType().GetTypeInfo().Assembly;

            var viewRenderer = new CompositeViewRenderer(logger,
                new PrecompiledViewRenderer(logger),
                app.ApplicationServices.GetRequiredService<RazorViewRenderer>());

            var stormpathMiddleware = StormpathMiddleware.Create(new StormpathOwinOptions()
            {
                LibraryUserAgent = GetLibraryUserAgent(hostingAssembly),
                Configuration = suppliedConfiguration.Configuration,
                ViewRenderer = viewRenderer,
                Logger = logger
            });

            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    stormpathMiddleware.Initialize(next);
                    return stormpathMiddleware.Invoke;
                });
            });

            app.UseMiddleware<StormpathAuthenticationMiddleware>(Options.Create(new StormpathAuthenticationOptions() { AuthenticationScheme = "Cookie" }), logger);
            app.UseMiddleware<StormpathAuthenticationMiddleware>(Options.Create(new StormpathAuthenticationOptions() { AuthenticationScheme = "Bearer" }), logger);

            return app;
        }

        private static string GetLibraryUserAgent(Assembly hostingAssembly)
        {
            var libraryVersion = typeof(StormpathMiddleware).GetTypeInfo().Assembly.GetName().Version;
            var libraryToken = $"stormpath-aspnetcore/{libraryVersion.Major}.{libraryVersion.Minor}.{libraryVersion.Build}";

            var hostVersion = hostingAssembly.GetName().Version;
            var hostToken = $"aspnetcore/{hostVersion.Major}.{hostVersion.Minor}.{hostVersion.Build}";

            return string.Join(" ", libraryToken, hostToken);
        }
    }
}
