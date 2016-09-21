// <copyright file="ScopedApplicationAccessor.cs" company="Stormpath, Inc.">
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

using Microsoft.AspNetCore.Http;
using Stormpath.Configuration.Abstractions.Immutable;
using Stormpath.Owin.Abstractions;
using Stormpath.SDK.Application;
using Stormpath.SDK.Client;
using Stormpath.SDK.Sync;

namespace Stormpath.AspNetCore
{
    internal sealed class ScopedApplicationAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ScopedApplicationAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public IApplication GetItem()
        {
            var context = _httpContextAccessor.HttpContext;
            var client = context.Items[OwinKeys.StormpathClient] as IClient;
            var configuration = context.Items[OwinKeys.StormpathConfiguration] as StormpathConfiguration;

            return configuration == null ? null : client.GetApplication(configuration.Application.Href);
        }
    }
}
