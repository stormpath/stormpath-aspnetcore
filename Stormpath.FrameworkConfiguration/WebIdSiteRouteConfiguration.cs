namespace Stormpath.Framework.Configuration
{
    public sealed class WebIdSiteRouteConfiguration : WebRouteWithNextConfigurationBase
    {
        public string LoginUri { get; private set; }

        public string ForgotUri { get; private set; }

        public string RegisterUri { get; private set; }
    }
}