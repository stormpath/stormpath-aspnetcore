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
    public class CustomDataRequirementShould
    {
        private readonly StandaloneTestFixture _fixture;

        public CustomDataRequirementShould(StandaloneTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task RedirectBrowserRequestWithoutCustomData()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("CustomDataIT",
                            policy => policy.AddRequirements(new StormpathCustomDataRequirement("testing", "rocks!")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(RedirectBrowserRequestWithoutCustomData),
                    nameof(CustomDataRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireCustomData");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            }
        }

        [Fact]
        public async Task ReturnUnauthorizedForJsonRequestWithoutCustomData()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("CustomDataIT",
                            policy => policy.AddRequirements(new StormpathCustomDataRequirement("testing", "rocks!")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(ReturnUnauthorizedForJsonRequestWithoutCustomData),
                    nameof(CustomDataRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireCustomData");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task AllowBrowserRequestWithMatchingCustomData()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("CustomDataIT",
                            policy => policy.AddRequirements(new StormpathCustomDataRequirement("testing", "rocks!")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestWithMatchingCustomData),
                    nameof(CustomDataRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                account.CustomData["testing"] = "rocks!";
                await account.SaveAsync();

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireCustomData");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task AllowJsonRequestWithMatchingCustomData()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture,
                services =>
                {
                    services.AddAuthorization(opt =>
                    {
                        opt.AddPolicy("CustomDataIT",
                            policy => policy.AddRequirements(new StormpathCustomDataRequirement("testing", "rocks!")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowJsonRequestWithMatchingCustomData),
                    nameof(CustomDataRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                account.CustomData["testing"] = "rocks!";
                await account.SaveAsync();

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/requireCustomData");
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
                        opt.AddPolicy("CustomDataIT",
                            policy => policy.AddRequirements(new StormpathCustomDataRequirement("testing", "rocks!")));
                    });
                });

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account1 = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(HandleConcurrentRequests),
                    nameof(CustomDataRequirementShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account1);

                account1.CustomData["testing"] = "rocks!";
                await account1.SaveAsync();

                var account2 = await _fixture.TestApplication.CreateAccountAsync(
                    $"{nameof(HandleConcurrentRequests)} #2",
                    nameof(CustomDataRequirementShould),
                    $"its-{_fixture.TestKey}-2@example.com",
                    "Changeme123!!");
                cleanup.MarkForDeletion(account2);

                var accessToken1 = await _fixture.GetAccessToken(account1, "Changeme123!!");
                var accessToken2 = await _fixture.GetAccessToken(account2, "Changeme123!!");

                var request1 = new HttpRequestMessage(HttpMethod.Get, "/requireCustomData");
                request1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken1);

                var request2 = new HttpRequestMessage(HttpMethod.Get, "/requireCustomData");
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
