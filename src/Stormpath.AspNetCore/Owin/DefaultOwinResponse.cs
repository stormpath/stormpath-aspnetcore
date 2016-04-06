// <copyright file="DefaultOwinResponse.cs" company="Stormpath, Inc.">
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

// Contains code copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stormpath.AspNetCore.Internal;

namespace Stormpath.AspNetCore.Owin
{
    public class DefaultOwinResponse : IOwinResponse
    {
        private readonly IDictionary<string, object> environment;

        public DefaultOwinResponse(IDictionary<string, object> owinEnvironment)
        {
            this.environment = owinEnvironment;
        }

        public Stream Body
        {
            get { return environment.Get<Stream>(OwinKeys.ResponseBody); }
            set { environment.SetOrRemove(OwinKeys.ResponseBody, value); }
        }

        public IDictionary<string, string[]> Headers
        {
            get { return environment.Get<IDictionary<string, string[]>>(OwinKeys.ResponseHeaders); }
            set { environment.SetOrRemove(OwinKeys.ResponseHeaders, value); }
        }

        public int StatusCode
        {
            set { environment.SetOrRemove(OwinKeys.ResponseStatusCode, value); }
        }

        public string ReasonPhrase
        {
            set { environment.SetOrRemove(OwinKeys.ResponseReasonPhrase, value); }
        }

        public Task WriteAsync(string text, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            byte[] data = encoding.GetBytes(text);
            return Body.WriteAsync(data, 0, data.Length, cancellationToken);
        }
    }
}
