services.AddStormpath(new StormpathConfiguration()
{
    Web = new WebConfiguration()
    {
        ForgotPassword = new WebForgotPasswordRouteConfiguration()
        {
            Enabled = false
        },
        ChangePassword = new WebChangePasswordRouteConfiguration()
        {
            Enabled = false
        },
        Login = new WebLoginRouteConfiguration()
        {
            Enabled = false
        },
        Logout = new WebLogoutRouteConfiguration()
        {
            Enabled = false
        },
        Me = new WebMeRouteConfiguration()
        {
            Enabled = false
        },
        Oauth2 = new WebOauth2RouteConfiguration()
        {
            Enabled = false
        },
        Register = new WebRegisterRouteConfiguration()
        {
            Enabled = false
        },
        VerifyEmail = new WebVerifyEmailRouteConfiguration()
        {
            Enabled = false
        }
    }
});
