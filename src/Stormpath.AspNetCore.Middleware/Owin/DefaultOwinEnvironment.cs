using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            set { environment.Set(OwinKeys.CallCancelled, value); }
        }
    }
}
