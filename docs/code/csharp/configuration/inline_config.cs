services.AddStormpath(new StormpathConfiguration()
{
    Application = new ApplicationConfiguration()
    {
        Name = "My Application"
    },
    Web = new WebConfiguration()
    {
        Register = new WebRegisterRouteConfiguration()
        {
            Enabled = false
        }
    }
});
