using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Stormpath.Owin.CoreHarness
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [FromServices]
        public SDK.Client.IClient StormpathClient { get; set; }

        [FromServices]
        public Lazy<SDK.Account.IAccount> StormpathAccountSafe { get; set; }

        public async Task<string> Get()
        {
            var account = StormpathAccountSafe.Value;

            var app = await StormpathClient.GetApplicationAsync("https://api.stormpath.com/v1/applications/5GBFI3wqIpipk0QIr65wxh");

            var app2 = await StormpathClient.GetApplicationAsync("https://api.stormpath.com/v1/applications/5GBFI3wqIpipk0QIr65wxh");

            var prop = await app.GetDefaultAccountStoreAsync();

            return "hello world";
        }
    }
}
