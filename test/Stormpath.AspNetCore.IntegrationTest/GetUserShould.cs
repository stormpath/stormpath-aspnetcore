using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Stormpath.AspNetCore.IntegrationTest
{
    [Collection(nameof(IntegrationTestCollection))]
    public class GetUserShould
    {
        private readonly StandaloneTestFixture _fixture;

        public GetUserShould(StandaloneTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ReturnNullForUnauthenticatedRequests()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            // Act
            var response = await server.GetAsync("/user");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            (await response.Content.ReadAsStringAsync()).Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetUserAuthenticatedByHeader()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(GetUserAuthenticatedByHeader),
                    nameof(GetUserShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/user");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                (await response.Content.ReadAsStringAsync()).Should().Be(account.Href);
            }
        }

        [Fact]
        public async Task GetUserAuthenticatedByCookie()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(GetUserAuthenticatedByHeader),
                    nameof(GetUserShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request = new HttpRequestMessage(HttpMethod.Get, "/user");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Cookie", $"access_token={accessToken}");

                // Act
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                (await response.Content.ReadAsStringAsync()).Should().Be(account.Href);
            }
        }

        [Fact]
        public async Task HandleConcurrentMixedAuthenticationRequests()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(HandleConcurrentMixedAuthenticationRequests),
                    nameof(GetUserShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var accessToken = await _fixture.GetAccessToken(account, "Changeme123!!");

                var request1 = new HttpRequestMessage(HttpMethod.Get, "/user");
                request1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var request2 = new HttpRequestMessage(HttpMethod.Get, "/user");
                request2.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Act
                var responses = await Task.WhenAll(
                    server.SendAsync(request1),
                    server.SendAsync(request2));

                // Assert
                responses[0].StatusCode.Should().Be(HttpStatusCode.OK);
                (await responses[0].Content.ReadAsStringAsync()).Should().Be(account.Href);

                responses[1].StatusCode.Should().Be(HttpStatusCode.NoContent);
                (await responses[1].Content.ReadAsStringAsync()).Should().BeNullOrEmpty();
            }
        }

        [Fact]
        public async Task HandleConcurrentAuthenticatedRequests()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account1 = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(HandleConcurrentAuthenticatedRequests),
                    nameof(GetUserShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account1);

                var account2 = await _fixture.TestApplication.CreateAccountAsync(
                    $"{nameof(HandleConcurrentAuthenticatedRequests)} #2",
                    nameof(GetUserShould),
                    $"its-{_fixture.TestKey}-2@example.com",
                    "Changeme123!!");
                cleanup.MarkForDeletion(account2);

                var accessToken1 = await _fixture.GetAccessToken(account1, "Changeme123!!");
                var accessToken2 = await _fixture.GetAccessToken(account2, "Changeme123!!");

                var request1 = new HttpRequestMessage(HttpMethod.Get, "/user");
                request1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken1);

                var request2 = new HttpRequestMessage(HttpMethod.Get, "/user");
                request2.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken2);

                // Act
                var responses = await Task.WhenAll(
                    server.SendAsync(request1),
                    server.SendAsync(request2));

                // Assert
                responses[0].StatusCode.Should().Be(HttpStatusCode.OK);
                (await responses[0].Content.ReadAsStringAsync()).Should().Be(account1.Href);

                responses[1].StatusCode.Should().Be(HttpStatusCode.OK);
                (await responses[1].Content.ReadAsStringAsync()).Should().Be(account2.Href);
            }
        }
    }
}
