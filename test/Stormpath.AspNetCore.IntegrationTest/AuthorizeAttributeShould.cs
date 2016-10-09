using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Stormpath.SDK.Oauth;
using Xunit;

namespace Stormpath.AspNetCore.IntegrationTest
{
    [Collection(nameof(IntegrationTestCollection))]
    public class AuthorizeAttributeShould
    {
        private readonly StandaloneTestFixture _fixture;

        public AuthorizeAttributeShould(StandaloneTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task RedirectForUnauthenticatedBrowserRequest()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "/protected");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            var response = await server.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Found);
            response.Headers.Location.OriginalString.Should().StartWith("/login?");
        }

        [Fact]
        public async Task ReturnUnauthorizedForUnauthenticatedJsonRequest()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "/protected");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await server.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task AllowBrowserRequestAuthorizedWithHeader()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestAuthorizedWithHeader),
                    nameof(AuthorizeAttributeShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var grantRequest = OauthRequests.NewPasswordGrantRequest()
                    .SetAccountStore(_fixture.TestDirectory)
                    .SetLogin(email)
                    .SetPassword("Changeme123!!")
                    .Build();
                var grantResponse = await _fixture.TestApplication.NewPasswordGrantAuthenticator()
                    .AuthenticateAsync(grantRequest);

                // Act
                var request = new HttpRequestMessage(HttpMethod.Get, "/protected");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", grantResponse.AccessTokenString);
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task AllowBrowserRequestAuthorizedWithCookie()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestAuthorizedWithHeader),
                    nameof(AuthorizeAttributeShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var grantRequest = OauthRequests.NewPasswordGrantRequest()
                    .SetAccountStore(_fixture.TestDirectory)
                    .SetLogin(email)
                    .SetPassword("Changeme123!!")
                    .Build();
                var grantResponse = await _fixture.TestApplication.NewPasswordGrantAuthenticator()
                    .AuthenticateAsync(grantRequest);

                // Act
                var request = new HttpRequestMessage(HttpMethod.Get, "/protected");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request.Headers.Add("Cookie", $"access_token={grantResponse.AccessTokenString}");
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task AllowJsonRequestAuthorizedWithHeader()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestAuthorizedWithHeader),
                    nameof(AuthorizeAttributeShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var grantRequest = OauthRequests.NewPasswordGrantRequest()
                    .SetAccountStore(_fixture.TestDirectory)
                    .SetLogin(email)
                    .SetPassword("Changeme123!!")
                    .Build();
                var grantResponse = await _fixture.TestApplication.NewPasswordGrantAuthenticator()
                    .AuthenticateAsync(grantRequest);

                // Act
                var request = new HttpRequestMessage(HttpMethod.Get, "/protected");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", grantResponse.AccessTokenString);
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task AllowJsonRequestAuthorizedWithCookie()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestAuthorizedWithHeader),
                    nameof(AuthorizeAttributeShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var grantRequest = OauthRequests.NewPasswordGrantRequest()
                    .SetAccountStore(_fixture.TestDirectory)
                    .SetLogin(email)
                    .SetPassword("Changeme123!!")
                    .Build();
                var grantResponse = await _fixture.TestApplication.NewPasswordGrantAuthenticator()
                    .AuthenticateAsync(grantRequest);

                // Act
                var request = new HttpRequestMessage(HttpMethod.Get, "/protected");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Cookie", $"access_token={grantResponse.AccessTokenString}");
                var response = await server.SendAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task HandleConcurrentRequests()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            using (var cleanup = new AutoCleanup(_fixture.Client))
            {
                var email = $"its-{_fixture.TestKey}@example.com";
                var account = await _fixture.TestApplication.CreateAccountAsync(
                    nameof(AllowBrowserRequestAuthorizedWithHeader),
                    nameof(AuthorizeAttributeShould),
                    email,
                    "Changeme123!!");
                cleanup.MarkForDeletion(account);

                var grantRequest = OauthRequests.NewPasswordGrantRequest()
                    .SetAccountStore(_fixture.TestDirectory)
                    .SetLogin(email)
                    .SetPassword("Changeme123!!")
                    .Build();
                var grantResponse = await _fixture.TestApplication.NewPasswordGrantAuthenticator()
                    .AuthenticateAsync(grantRequest);

                // Act
                var request1 = new HttpRequestMessage(HttpMethod.Get, "/protected");
                request1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                request1.Headers.Add("Cookie", $"access_token={grantResponse.AccessTokenString}");

                var request2 = new HttpRequestMessage(HttpMethod.Get, "/protected");
                request2.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

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
