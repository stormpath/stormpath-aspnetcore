using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
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
    }
}
