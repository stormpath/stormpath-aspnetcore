using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Stormpath.Configuration.Abstractions;
using Stormpath.Owin.Middleware;
using Stormpath.SDK.Client;
using Stormpath.SDK.Http;
using Stormpath.SDK.Oauth;
using Stormpath.SDK.Serialization;
using Xunit;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Stormpath.AspNetCore.IntegrationTest
{
    [Collection(nameof(IntegrationTestCollection))]
    public class GroupsRequirementShould
    {
        private readonly StandaloneTestFixture _fixture;

        public GroupsRequirementShould(StandaloneTestFixture fixture)
        {
            _fixture = fixture;
        }

        private HttpClient CreateServer()
        {
            return TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("AdminITGroup", policy => policy.AddRequirements(new StormpathGroupsRequirement("admin")));
                    });

                },
                app =>
                {
                    var authorizationService = app.ApplicationServices.GetService<IAuthorizationService>();

                    app.Run(async context =>
                    {
                        if (await authorizationService.AuthorizeAsync(context.User, null, "AdminITGroup"))
                        {
                            await context.Response.WriteAsync("Hello, World!");
                        }
                        else
                        {
                            context.Response.StatusCode = 401; // TODO does 403 work?
                        }
                    });
                });
        }

        [Fact]
        public async Task RedirectUnauthenticatedBrowserRequest()
        {
            // Arrange
            var server = CreateServer();

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                // Act
                var request = new HttpRequestMessage(HttpMethod.Get, "/test");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Found);
            }
        }

        [Fact]
        public async Task RedirectUnauthenticatedJsonRequest()
        {
            // Arrange
            var server = CreateServer();

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                // Act
                var request = new HttpRequestMessage(HttpMethod.Get, "/test");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task AllowBrowserRequestWithCorrectGroup()
        {
            // Arrange
            var server = CreateServer();

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var group = await _fixture.TestDirectory.CreateGroupAsync("AdminITGroup", "Test group");
                cleanup.MarkForDeletion(group);

                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestWithCorrectGroup),
                    nameof(GroupsRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                await account.AddGroupAsync(group);

                var grantRequest = OauthRequests.NewPasswordGrantRequest()
                    .SetAccountStore(_fixture.TestDirectory)
                    .SetLogin(email)
                    .SetPassword("Changeme123!!")
                    .Build();
                var grantResponse = await _fixture.TestApplication.NewPasswordGrantAuthenticator()
                    .AuthenticateAsync(grantRequest);

                // Act
                var request = new HttpRequestMessage(HttpMethod.Get, "/test");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", grantResponse.AccessTokenString);
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
