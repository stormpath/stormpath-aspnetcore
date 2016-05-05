services.AddStormpath(new StormpathConfiguration()
{
    Web = new WebConfiguration()
    {
        Login = new WebLoginRouteConfiguration()
        {
            Form = new WebLoginRouteFormConfiguration()
            {
                Fields = new Dictionary<string, WebFieldConfiguration>()
                {
                    ["login"] = new WebFieldConfiguration()
                    {
                        Label = "Email",
                        Placeholder = "you@yourdomain.com"
                    },
                    ["password"] = new WebFieldConfiguration()
                    {
                        Placeholder = "Tip: Use a strong password!"
                    }
                }
            }
        }
    }
});
