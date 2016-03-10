using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stormpath.AspNetCore.Owin
{
    public sealed class DefaultOwinRequest : IOwinRequest
    {
        private readonly IDictionary<string, object> environment;

        public DefaultOwinRequest(IDictionary<string, object> owinEnvironment)
        {
            this.environment = owinEnvironment;
        }

        public Stream Body
            => environment.Get<Stream>(OwinKeys.RequestBody);

        public IDictionary<string, string[]> Headers
            => environment.Get<IDictionary<string, string[]>>(OwinKeys.RequestHeaders);

        public string Method
            => environment.Get<string>(OwinKeys.RequestMethod);

        public string Path
            => environment.Get<string>(OwinKeys.RequestPath);

        public string PathBase
            => environment.Get<string>(OwinKeys.RequestPathBase);
        
        public string QueryString
            => environment.Get<string>(OwinKeys.RequestQueryString);
    }
}
