using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            set { environment.Set(OwinKeys.ResponseBody, value); }
        }

        public IDictionary<string, string[]> Headers
        {
            get { return environment.Get<IDictionary<string, string[]>>(OwinKeys.ResponseHeaders); }
            set { environment.Set(OwinKeys.ResponseHeaders, value); }
        }

        public int StatusCode
        {
            set { environment.Set(OwinKeys.ResponseStatusCode, value); }
        }

        public string ReasonPhrase
        {
            set { environment.Set(OwinKeys.ResponseReasonPhrase, value); }
        }

    }
}
