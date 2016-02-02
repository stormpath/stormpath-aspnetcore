using System.Collections.Generic;

namespace Stormpath.Framework.Configuration
{
    public class WebLoginRouteFormConfiguration
    {
        public IDictionary<string, WebFieldConfiguration> Fields { get; set; }

        public IList<string> FieldOrder { get; set; }
    }
}