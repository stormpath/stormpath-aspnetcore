using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Stormpath.AspNetCore.IntegrationTest
{
    [Collection(nameof(IntegrationTestCollection))]
    public class GetConfigShould
    {
        private readonly StandaloneTestFixture _fixture;

        public GetConfigShould(StandaloneTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetFromContext()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            // Act
            var response = await server.GetAsync("/config");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            (await response.Content.ReadAsStringAsync()).Should().Be(_fixture.TestApplication.Name);
        }
    }
}
