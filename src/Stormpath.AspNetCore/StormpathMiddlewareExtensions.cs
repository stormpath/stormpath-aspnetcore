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
using Stormpath.Configuration.Abstractions;
using Stormpath.Owin.Middleware;
using Stormpath.Owin.Views.Precompiled;

namespace Stormpath.AspNetCore
{
    public static class StormpathMiddlewareExtensions
    {
        /// <summary>
        /// Adds services required for Stormpath.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="Stormpath.Owin.InitializationException">There was a problem initializing Stormpath.</exception>
        public static IServiceCollection AddStormpath(
            this IServiceCollection services)
        {
            return AddStormpath(services, new StormpathOwinOptions());
        }

        /// <summary>
        /// Adds services required for Stormpath.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">Configuration for the Stormpath middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="Stormpath.Owin.InitializationException">There was a problem initializing Stormpath.</exception>
        public static IServiceCollection AddStormpath(
            this IServiceCollection services,
            StormpathConfiguration configuration)
        {
            return AddStormpath(services, new StormpathOwinOptions
            {
                Configuration = configuration
            });
        }

        /// <summary>
        /// Adds services required for Stormpath.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="anonymousConfiguration">Configuration for the Stormpath middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="Stormpath.Owin.InitializationException">There was a problem initializing Stormpath.</exception>
        public static IServiceCollection AddStormpath(
            this IServiceCollection services,
            object anonymousConfiguration)
        {
            return AddStormpath(services, new StormpathOwinOptions
            {
                Configuration = anonymousConfiguration
            });
        }

        /// <summary>
        /// Adds services required for Stormpath.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="options">Extended configuration for the Stormpath middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="Stormpath.Owin.InitializationException">There was a problem initializing Stormpath.</exception>
        public static IServiceCollection AddStormpath(
            this IServiceCollection services,
            StormpathOptions options)
        {
            var owinOptions = new StormpathOwinOptions
            {
                Configuration = options?.Configuration,
                CacheProvider = options?.CacheProvider,
                PostChangePasswordHandler = options?.PostChangePasswordHandler,
                PostLoginHandler = options?.PostLoginHandler,
                PostLogoutHandler = options?.PostLogoutHandler,
                PostRegistrationHandler = options?.PostRegistrationHandler,
                PostVerifyEmailHandler = options?.PostVerifyEmailHandler,
                PreChangePasswordHandler = options?.PreChangePasswordHandler,
                PreLoginHandler = options?.PreLoginHandler,
                PreLogoutHandler = options?.PreLogoutHandler,
                PreRegistrationHandler = options?.PreRegistrationHandler,
                PreVerifyEmailHandler = options?.PreVerifyEmailHandler,
            };

            return AddStormpath(services, owinOptions);
        }

        private static IServiceCollection AddStormpath(
            IServiceCollection services,
            StormpathOwinOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddLogging();
            services.AddSingleton<SDK.Logging.ILogger>(
                provider => new LoggerAdapter(provider.GetRequiredService<ILoggerFactory>()));

            services.AddSingleton(new OptionsContainer(options));

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

            return services;
        }

        /// <summary>
        /// Adds the Stormpath middleware to the pipeline.
        /// </summary>
        /// <remarks>You must call <c>AddStormpath</c> before calling this method.</remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="InvalidOperationException">The Stormpath services have not been added to the service collection.</exception>
        public static IApplicationBuilder UseStormpath(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var suppliedConfiguration = app.ApplicationServices.GetRequiredService<OptionsContainer>();
            var logger = app.ApplicationServices.GetRequiredService<SDK.Logging.ILogger>();

            var hostingAssembly = app.GetType().GetTypeInfo().Assembly;

            var viewRenderer = new CompositeViewRenderer(logger,
                new PrecompiledViewRenderer(logger),
                app.ApplicationServices.GetRequiredService<RazorViewRenderer>());

            var options = suppliedConfiguration.Options;
            options.LibraryUserAgent = GetLibraryUserAgent(hostingAssembly);
            options.ViewRenderer = viewRenderer;
            options.Logger = logger;

            var stormpathMiddleware = StormpathMiddleware.Create(options);

            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    stormpathMiddleware.Initialize(next);
                    return stormpathMiddleware.Invoke;
                });
            });

            app.UseMiddleware<StormpathAuthenticationMiddleware>(
                Options.Create(new StormpathAuthenticationOptions { AllowedAuthenticationSchemes = new [] { "Cookie", "Bearer" } }),
                stormpathMiddleware.Configuration,
                logger);

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
