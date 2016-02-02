namespace Stormpath.Framework.Configuration
{
    public class WebChangePasswordRouteConfiguration : WebRouteWithNextConfigurationBase
    {
        public bool AutoLogin { get; private set; }

        public string View { get; private set; }

        public string ErrorUri { get; private set; }
    }
}