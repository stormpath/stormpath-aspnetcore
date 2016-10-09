using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Stormpath.AspNetCore.IntegrationTest
{
    [Collection(nameof(IntegrationTestCollection))]
    public class GetClientShould
    {
        private readonly StandaloneTestFixture _fixture;

        public GetClientShould(StandaloneTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetClientFromContext()
        {
            // Arrange
            var server = TestServerInstance.Create(_fixture);

            // Act
            var response = await server.GetAsync("/client");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var expectedTenant = await _fixture.TestApplication.GetTenantAsync();
            (await response.Content.ReadAsStringAsync()).Should().Be(expectedTenant.Href);
        }
    }
}
