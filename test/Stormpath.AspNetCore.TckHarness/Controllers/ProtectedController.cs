// <copyright file="SecretController.cs" company="Stormpath, Inc.">
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stormpath.Configuration.Abstractions.Immutable;
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.TestHarness.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProtectedController
    {
        private readonly IClient _client;
        private readonly IApplication _application;
        private readonly StormpathConfiguration _configuration;
        private readonly IAccount _account;

        public ProtectedController(IClient client, IApplication application, StormpathConfiguration config, Lazy<IAccount> lazyAccount)
        {
            _client = client;
            _application = application;
            _configuration = config;
            _account = lazyAccount.Value;
        }

        public async Task<string> Get()
        {
            var tenant = await _client.GetCurrentTenantAsync();

            return "Hello secure world";
        }
    }
}
