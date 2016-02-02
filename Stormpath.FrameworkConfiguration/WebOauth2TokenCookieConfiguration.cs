namespace Stormpath.Framework.Configuration
{
    public sealed class WebOauth2TokenCookieConfiguration
    {
        public string Name { get; private set; }

        public bool HttpOnly { get; private set; }

        public bool? Secure { get; private set; }

        public string Path { get; private set; }

        public string Domain { get; private set; }
    }
}