services.AddStormpath(new StormpathConfiguration()
{
    Web = new WebConfiguration()
    {
        Logout = new WebLogoutRouteConfiguration()
        {
            Uri = "/logMeOut",
            NextUri = "/goodbye"
        }
    }
});
