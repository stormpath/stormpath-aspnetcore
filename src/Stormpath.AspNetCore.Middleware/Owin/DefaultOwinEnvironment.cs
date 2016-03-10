// <copyright file="DefaultOwinEnvironment.cs" company="Stormpath, Inc.">
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

using System.Collections.Generic;
using System.Threading;
using Stormpath.AspNetCore.Internal;

namespace Stormpath.AspNetCore.Owin
{
    public sealed class DefaultOwinEnvironment : IOwinEnvironment
    {
        private readonly IDictionary<string, object> environment;

        public DefaultOwinEnvironment(IDictionary<string, object> owinEnvironment)
        {
            this.environment = owinEnvironment;
            this.Request = new DefaultOwinRequest(owinEnvironment);
            this.Response = new DefaultOwinResponse(owinEnvironment);
        }

        public IOwinRequest Request { get; private set; }

        public IOwinResponse Response { get; private set; }

        public CancellationToken CancellationToken
        {
            get { return environment.Get<CancellationToken>(OwinKeys.CallCancelled); }
            set { environment.SetOrRemove(OwinKeys.CallCancelled, value); }
        }
    }
}
