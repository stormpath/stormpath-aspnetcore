using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.IntegrationTest
{
    public static class TestServerInstance
    {
        public static HttpClient Create(
            StandaloneTestFixture fixture, 
            Action<IServiceCollection> customConfigureServices,
            Action<IApplicationBuilder> customConfigureApp)
        {
            return new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddStormpath(new StormpathConfiguration()
                    {
                        Application = new ApplicationConfiguration()
                        {
                            Href = fixture.TestApplication.Href
                        }
                    });
                    services.AddMvc();
                    customConfigureServices(services);
                })
                .Configure(app =>
                {
                    app.UseStormpath();
                    customConfigureApp(app);
                }))
                .CreateClient();
        }
    }
}
