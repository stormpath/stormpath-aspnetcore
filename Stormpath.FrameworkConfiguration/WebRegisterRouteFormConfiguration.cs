using System.Collections.Generic;

namespace Stormpath.Framework.Configuration
{
    public sealed class WebRegisterRouteFormConfiguration
    {
        public IDictionary<string, WebFieldConfiguration> Fields { get; set; }

        public IReadOnlyList<string> FieldOrder { get; set; }
    }
}