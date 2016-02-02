namespace Stormpath.Framework.Configuration
{
    public sealed class WebRegisterRouteConfiguration : WebRouteWithNextConfigurationBase
    {
        public bool AutoLogin { get; private set; }

        public WebRegisterRouteFormConfiguration Form { get; private set; }

        public string View { get; set; }
    }
}