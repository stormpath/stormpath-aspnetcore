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
using Stormpath.SDK.Application;
using Stormpath.SDK.Sync;

namespace Stormpath.AspNetCore
{
    internal sealed class ScopedApplicationAccessor
    {
        private readonly ScopedClientAccessor _clientAccessor;
        private readonly ScopedConfigurationAccessor _configAccessor;

        public ScopedApplicationAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _clientAccessor = new ScopedClientAccessor(httpContextAccessor);
            _configAccessor = new ScopedConfigurationAccessor(httpContextAccessor);
        }

        public IApplication Item
        {
            get
            {
                var client = _clientAccessor.Item;
                var config = _configAccessor.Item;

                if (client == null || config == null)
                {
                    return null;
                }

                return client.GetApplication(config.Application.Href);
            }
        }
    }
}
