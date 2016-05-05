services.AddStormpath(new StormpathConfiguration()
{
    Web = new WebConfiguration()
    {
        Oauth2 = new WebOauth2RouteConfiguration()
        {
            Uri = "/api/token",
            Password = new WebOauth2PasswordGrantConfiguration()
            {
                ValidationStrategy = WebOauth2TokenValidationStrategy.Stormpath
            }
        }
    }
});
