// <copyright file="UserAgentMiddleware.cs" company="Stormpath, Inc.">
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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.TestHost;
using Xunit;

namespace Stormpath.AspNetCore.Tests.Integration
{
    public class UserAgentMiddleware
    {
        [Fact]
        public async Task Missing_agent_header_is_ignored()
        {
            var verifier = new Func<HttpContext, Func<Task>, Task>((ctx, next) =>
            {
                (ctx.Items["StormpathAgent"] as string).Should().BeNullOrEmpty();
                ctx.Response.StatusCode = (int)HttpStatusCode.Created;
                return Task.FromResult(0);
            });

            var client = CreateClient(verifier);

            var response = await client.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Agent_header_is_put_into_items_collection()
        {
            var verifier = new Func<HttpContext, Func<Task>, Task>((ctx, next) =>
            {
                (ctx.Items["StormpathAgent"] as string).Should().Be("foo/123");
                ctx.Response.StatusCode = (int)HttpStatusCode.Created;
                return Task.FromResult(0);
            });

            var client = CreateClient(verifier);
            client.DefaultRequestHeaders.Add("X-Stormpath-Agent", "foo/123");

            var response = await client.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Matching_is_case_insensitive()
        {
            var verifier = new Func<HttpContext, Func<Task>, Task>((ctx, next) =>
            {
                (ctx.Items["StormpathAgent"] as string).Should().Be("bar/123");
                ctx.Response.StatusCode = (int)HttpStatusCode.Created;
                return Task.FromResult(0);
            });

            var client = CreateClient(verifier);
            client.DefaultRequestHeaders.Add("x-stormpath-agent", "bar/123");

            var response = await client.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handles_multiple_strings()
        {
            var verifier = new Func<HttpContext, Func<Task>, Task>((ctx, next) =>
            {
                (ctx.Items["StormpathAgent"] as string).Should().Be("foo/123 bar/456");
                ctx.Response.StatusCode = (int)HttpStatusCode.Created;
                return Task.FromResult(0);
            });

            var client = CreateClient(verifier);
            client.DefaultRequestHeaders.Add("X-Stormpath-Agent", new string[] { "foo/123", "bar/456" });

            var response = await client.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        private static HttpClient CreateClient(Func<HttpContext, Func<Task>, Task> verifier)
        {
            var server = new TestServer(TestServer.CreateBuilder()
                .UseStartup(app =>
                {
                    app.UseMiddleware<StormpathUserAgentMiddleware>();
                    app.Use(verifier);
                }));

            return server.CreateClient();
        }
    }
}
