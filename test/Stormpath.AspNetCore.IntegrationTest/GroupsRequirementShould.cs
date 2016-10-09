using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

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

        [Fact]
        public async Task RedirectBrowserRequestWithoutGroup()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("AdminITGroup",
                            policy => policy.AddRequirements(new StormpathGroupsRequirement("adminIT")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestWithCorrectGroup),
                    nameof(GroupsRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireGroup");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            }
        }

        [Fact]
        public async Task ReturnUnauthorizedForJsonRequestWithoutGroup()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("AdminITGroup",
                            policy => policy.AddRequirements(new StormpathGroupsRequirement("adminIT")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestWithCorrectGroup),
                    nameof(GroupsRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireGroup");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task AllowBrowserRequestWithCorrectGroup()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("AdminITGroup",
                            policy => policy.AddRequirements(new StormpathGroupsRequirement("adminIT")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var group = await _fixture.TestDirectory.CreateGroupAsync("adminIT", "Stormpath.AspNetCore test group");
                cleanup.MarkForDeletion(group);

                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestWithCorrectGroup),
                    nameof(GroupsRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                await account.AddGroupAsync(group);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireGroup");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task AllowJsonRequestWithCorrectGroup()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("AdminITGroup",
                            policy => policy.AddRequirements(new StormpathGroupsRequirement("adminIT")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var group = await _fixture.TestDirectory.CreateGroupAsync("adminIT", "Stormpath.AspNetCore test group");
                cleanup.MarkForDeletion(group);

                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestWithCorrectGroup),
                    nameof(GroupsRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                await account.AddGroupAsync(group);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireGroup");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task HandleConcurrentRequests()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("AdminITGroup",
                            policy => policy.AddRequirements(new StormpathGroupsRequirement("adminIT")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var group = await _fixture.TestDirectory.CreateGroupAsync("adminIT", "Stormpath.AspNetCore test group");
                cleanup.MarkForDeletion(group);

                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(HandleConcurrentRequests),
                    nameof(GroupsRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);
                await account.AddGroupAsync(group);

                var account2 = await _fixture.TestApplication.CreateAccountAsync(
                    $"{nameof(HandleConcurrentRequests)} #2",
                    nameof(GroupsRequirementShould),
                    $"its-{_fixture.TestKey}-2@example.com",
                    "Changeme123!!");
                cleanup.MarkForDeletion(account2);

                var accessToken1 = await _fixture.GetAccessToken(account, "Changeme123!!");
                var accessToken2 = await _fixture.GetAccessToken(account2, "Changeme123!!");

                var request1 = new HttpRequestMessage(HttpMethod.Get, "/requireGroup");
                request1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken1);

                var request2 = new HttpRequestMessage(HttpMethod.Get, "/requireGroup");
                request2.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken2);

                // Act
                var responses = await Task.WhenAll(
                    server.SendAsync(request1),
                    server.SendAsync(request2));

                // Assert
                responses[0].StatusCode.Should().Be(HttpStatusCode.OK);

                responses[1].StatusCode.Should().Be(HttpStatusCode.Redirect);
            }
        }
    }
}
