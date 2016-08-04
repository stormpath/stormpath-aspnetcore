namespace Stormpath.AspNetCore
{
    public sealed class StormpathMiddlewareOptions
    {
        public object Configuration { get; set; }

        public IIdentityTransformer IdentityTransformer { get; set; }
    }
}