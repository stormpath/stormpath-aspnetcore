// <copyright file="OpenController.cs" company="Stormpath, Inc.">
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
using Microsoft.AspNetCore.Mvc;

namespace Stormpath.AspNetCore.TestHarness.Controllers
{
    [Route("api/[controller]")]
    public class OpenController : Controller
    {
        private readonly SDK.Client.IClient stormpathClient;
        private readonly Lazy<SDK.Account.IAccount> stormpathAccountSafe;

        public OpenController(SDK.Client.IClient client, Lazy<SDK.Account.IAccount> account)
        {
            this.stormpathClient = client;
            this.stormpathAccountSafe = account;
        }

        public async Task<string> Get()
        {
            var tenant = await stormpathClient.GetCurrentTenantAsync();

            return "Hello world";
        }
    }
}
