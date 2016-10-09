using System;
using System.Threading.Tasks;
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;
using Stormpath.SDK.Client;
using Stormpath.SDK.Directory;
using Stormpath.SDK.Http;
using Stormpath.SDK.Oauth;
using Stormpath.SDK.Resource;
using Stormpath.SDK.Serialization;

namespace Stormpath.AspNetCore.IntegrationTest
{
    public class StandaloneTestFixture : IDisposable
    {
        private readonly AutoCleanup _environment;

        public StandaloneTestFixture()
            : this(Clients.Builder()
                .SetHttpClient(HttpClients.Create().SystemNetHttpClient())
                .SetSerializer(Serializers.Create().JsonNetSerializer())
                .Build())
        {
        }

        public StandaloneTestFixture(IClient client)
        {
            Client = client;
            TestKey = Guid.NewGuid().ToString();

            _environment = new AutoCleanup(Client, async c =>
            {
                TestApplication = await c.CreateApplicationAsync($"Stormpath.AspNetCore IT {TestKey}", true);
                TestDirectory = await TestApplication.GetDefaultAccountStoreAsync() as IDirectory;
                return new IResource[] {TestApplication, TestDirectory};
            });
        }

        public async Task<string> GetAccessToken(IAccount account, string password)
        {
            var grantRequest = OauthRequests.NewPasswordGrantRequest()
                .SetLogin(account.Email)
                .SetPassword(password)
                .Build();
            var grantResponse = await TestApplication.NewPasswordGrantAuthenticator()
                .AuthenticateAsync(grantRequest);

            return grantResponse.AccessTokenString;
        }

        public IClient Client { get; }

        public string TestKey { get; }

        public IApplication TestApplication { get; private set; }

        public IDirectory TestDirectory { get; private set; }

        public void Dispose()
        {
            _environment.Dispose();
        }
    }
}