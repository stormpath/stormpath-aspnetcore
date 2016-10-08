using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stormpath.AspNetCore.IntegrationTest
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public class IntegrationTestCollection : ICollectionFixture<StandaloneTestFixture>
    {
        // Intentionally left blank. This class only serves as an anchor for CollectionDefinitionAttribute.
    }
}
