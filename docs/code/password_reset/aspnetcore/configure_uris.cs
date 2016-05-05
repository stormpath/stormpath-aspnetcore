services.AddStormpath(new StormpathConfiguration
{
    Web = new WebConfiguration
    {
        ForgotPassword = new WebForgotPasswordRouteConfiguration
        {
            Uri = "/forgot-password"
        },
        ChangePassword = new WebChangePasswordRouteConfiguration
        {
            Uri = "/change-password"
        }
    }
});
