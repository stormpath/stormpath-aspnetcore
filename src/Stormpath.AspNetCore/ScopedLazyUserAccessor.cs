// <copyright file="StormpathContextLazyUserAccessor.cs" company="Stormpath, Inc.">
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
using Microsoft.AspNetCore.Http;
using Stormpath.Owin.Abstractions;
using Stormpath.SDK.Account;

namespace Stormpath.AspNetCore
{
    internal sealed class ScopedLazyUserAccessor
    {
        private readonly Lazy<IAccount> account;

        public ScopedLazyUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.account = new Lazy<IAccount>(() =>
            {
                var context = httpContextAccessor.HttpContext;
                return context.Items[OwinKeys.StormpathUser] as IAccount;
            });
        }

        public Lazy<IAccount> GetItem() => this.account;
    }
}
