services.AddStormpath(new StormpathConfiguration()
{
    Web = new WebConfiguration()
    {
        VerifyEmail = new WebVerifyEmailRouteConfiguration()
        {
            Uri = "/verifyEmail"
        }
    }
});
