namespace Stormpath.AspNetCore.Owin
{
    /// <summary>
    /// OWIN keys, as defined in <see href="http://owin.org/html/spec/owin-1.0.html"/>.
    /// </summary>
    public static class OwinKeys
    {
        public static readonly string RequestBody = "owin.RequestBody";

        public static readonly string RequestHeaders = "owin.RequestHeaders";

        public static readonly string RequestMethod = "owin.RequestMethod";

        public static readonly string RequestPath = "owin.RequestPath";

        public static readonly string RequestPathBase = "owin.RequestPathBase";

        public static readonly string RequestProtocol = "owin.RequestProtocol";

        public static readonly string RequestQueryString = "owin.RequestQueryString";

        public static readonly string RequestScheme = "owin.RequestScheme";

        public static readonly string RequestUser = "owin.RequestUser";

        public static readonly string RequestUserOld = "server.User";

        public static readonly string ResponseBody = "owin.ResponseBody";

        public static readonly string ResponseHeaders = "owin.ResponseHeaders";

        public static readonly string ResponseStatusCode = "owin.ResponseStatusCode";

        public static readonly string ResponseReasonPhrase = "owin.ResponseReasonPhrase";

        public static readonly string ResponseProtocol = "owin.ResponseProtocol";

        public static readonly string CallCancelled = "owin.CallCancelled";
    }
}
