services.AddStormpath(new StormpathConfiguration
{
    Web = new WebConfiguration
    {
        Register = new WebRegisterRouteConfiguration
        {
            Form = new WebRegisterRouteFormConfiguration
            {
                Fields = new Dictionary<string, WebFieldConfiguration>
                {
                    ["favoriteColor"] = new WebFieldConfiguration
                    {
                        Enabled = true,
                        Visible = true,
                        Label = "Favorite Color",
                        Placeholder = "e.g. red, blue",
                        Required = true,
                        Type = "text"
                    }
                }
            }
        }
    }
});
