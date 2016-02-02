namespace Stormpath.Framework.Configuration
{
    public sealed class WebForgotPasswordRouteConfiguration : WebRouteWithNextConfigurationBase
    {
        public string View { get; private set; }
    }
}