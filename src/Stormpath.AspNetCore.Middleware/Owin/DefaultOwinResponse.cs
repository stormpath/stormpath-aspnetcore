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
