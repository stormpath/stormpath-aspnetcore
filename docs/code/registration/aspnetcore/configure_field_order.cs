services.AddStormpath(new StormpathConfiguration
{
    Web = new WebConfiguration
    {
        Register = new WebRegisterRouteConfiguration
        {
            Form = new WebRegisterRouteFormConfiguration
            {
                FieldOrder = new string[]
                {
                    "surname",
                    "givenName",
                    "email",
                    "password"
                }
            }
        }
    }
});
