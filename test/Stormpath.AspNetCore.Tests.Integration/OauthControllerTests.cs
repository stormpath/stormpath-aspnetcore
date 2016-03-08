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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.TestHost;
using Stormpath.AspNetCore.Routes;
using Xunit;

namespace Stormpath.AspNetCore.Tests.Integration
{
    public class OauthControllerTests
    {
        [Fact]
        public async Task Does_not_respond_to_get()
        {
            var client = CreateClient();

            var response = await client.GetAsync("/oauth/tokens");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound); // 404
        }

        [Fact]
        public async Task Does_not_respond_to_post_without_form()
        {
            var client = CreateClient();

            var jsonRequest = new StringContent(@"{ ""hello"" : ""world"" }", Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/oauth/tokens", jsonRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest); // 400
        }

        [Fact]
        public async Task Returns_invalid_request_for_empty_grant_type()
        {
            var client = CreateClient();

            var grantRequest = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["grant_type"] = "",
                ["username"] = "foo",
                ["password"] = "bar"
            });

            var response = await client.PostAsync("/oauth/tokens", grantRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest); // 400
            var responseContent = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task Executes_password_grant_flow()
        {
            var client = CreateClient();

            var grantRequest = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["grant_type"] = "password",
                ["username"] = "foo",
                ["password"] = "bar"
            });

            var response = await client.PostAsync("/oauth/tokens", grantRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest); // 400
        }

        private static HttpClient CreateClient(Func<HttpContext, Func<Task>, Task> finalizer = null)
        {
            var server = new TestServer(TestServer.CreateBuilder()
                .UseServices(services =>
                {
                    services.AddStormpath();
                })
                .UseStartup(app =>
                {
                    app.UseMiddleware<Oauth2Route>("/oauth/tokens");

                    if (finalizer != null)
                    {
                        app.Use(finalizer);
                    }
                }));

            return server.CreateClient();
        }
    }
}
