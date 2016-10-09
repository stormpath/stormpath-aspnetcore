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
    }
}
