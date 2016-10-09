using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Stormpath.AspNetCore.IntegrationTest
{
    [Collection(nameof(IntegrationTestCollection))]
    public class GetApplicationShould
    {
        private readonly StandaloneTestFixture _fixture;

        public GetApplicationShould(StandaloneTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetFromContext()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            // Act
            var response = await server.GetAsync("/application");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            (await response.Content.ReadAsStringAsync()).Should().Be(_fixture.TestApplication.Href);
        }
    }
}
