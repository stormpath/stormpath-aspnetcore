// <copyright file="RegisterRoute.cs" company="Stormpath, Inc.">
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
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Stormpath.AspNetCore.Model.Error;
using Stormpath.Configuration.Abstractions;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.Route
{
    public sealed class RegisterRoute : AbstractRouteMiddleware
    {
        private readonly static string[] SupportedMethods = { "POST" };
        private readonly static string[] SupportedContentTypes = { "application/json" }; // todo

        public RegisterRoute(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IScopedClientFactory clientFactory,
            StormpathConfiguration configuration,
            string path)
            : base(next, loggerFactory, clientFactory, configuration, path, SupportedMethods, SupportedContentTypes)
        {
        }

        protected override Task PostJson(HttpContext context, IClient scopedClient)
        {
            throw new NotImplementedException();
        }
    }
}
