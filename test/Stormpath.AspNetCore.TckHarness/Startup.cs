// <copyright file="Startup.cs" company="Stormpath, Inc.">
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

using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.TestHarness
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Stormpath services
            var stormpathConfiguration = new StormpathConfiguration
            {
                Org = "https://dev-123456.oktapreview.com",
                ApiToken = "your_token_here",
                Application = new OktaApplicationConfiguration
                {
                    Id = "abc123"
                },
                Web = new WebConfiguration
                {
                    ServerUri = "http://localhost:8080/",
                    ChangePassword = new WebChangePasswordRouteConfiguration
                    {
                        Enabled = true
                    },
                    ForgotPassword = new WebForgotPasswordRouteConfiguration
                    {
                        Enabled = true
                    }
                }
            };
            services.AddStormpath(stormpathConfiguration);

            // Configure authorization policies here, which can include Stormpath requirements.
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("CustomDataIT", policy => policy.AddRequirements(new StormpathCustomDataRequirement("testing", "rocks!")));
                opt.AddPolicy("AdminITGroup", policy => policy.AddRequirements(new StormpathGroupsRequirement("adminIT")));
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Use Stormpath middleware
            app.UseStormpath();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
