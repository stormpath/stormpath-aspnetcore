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
                    ["givenName"] = new WebFieldConfiguration { Required = false },
                    ["surname"] = new WebFieldConfiguration { Required = false }
                }
            }
        }
    }
});
